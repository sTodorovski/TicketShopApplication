using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;
using TicketShop.Repository.Interface;
using TicketShop.Services.Interface;

namespace TicketShop.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        private readonly IRepository<EmailMessage> _emailMessageRepository;
        private readonly IUserRepository _userRepository;

        
        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<Order> orderRepository, IRepository<TicketInOrder> ticketInOrderRepository, IRepository<EmailMessage> emailMessageRepository, IUserRepository userRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
            _emailMessageRepository = emailMessageRepository;
            _userRepository = userRepository;
        }

        public bool deleteTicketFromShoppingCart(string userId, Guid id)
        {
            if(!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                userShoppingCart.TicketInShoppingCart.Remove(userShoppingCart.TicketInShoppingCart.Where(t => t.TicketId.Equals(id)).FirstOrDefault());

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }

            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.UserCart;

            var ticketPrice = userShoppingCart.TicketInShoppingCart.Select(t => new
            {
                TicketPrice = t.Ticket.Price,
                Quantity = t.Quantity
            }).ToList();

            var totalPrice = 0;

            foreach (var item in ticketPrice)
            {
                totalPrice += item.TicketPrice * item.Quantity;
            }

            ShoppingCartDto shoppingCartDtoItem = new ShoppingCartDto
            {
                TicketInShoppingCarts = userShoppingCart.TicketInShoppingCart.ToList(),
                TotalPrice = totalPrice,
            };

            return shoppingCartDtoItem;
        }

        public bool orderNow(string userId)
        {
            if(!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                EmailMessage message = new EmailMessage();
                message.MailTo = loggedInUser.Email;
                message.Subject = "Your order has been successfully created!";
                message.Status = false;
                

                Order orderItem = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    User = loggedInUser
                };

                this._orderRepository.Insert(orderItem);

                List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

                var result = userShoppingCart.TicketInShoppingCart.Select(t => new TicketInOrder
                    {
                        OrderId = orderItem.Id,
                        TicketId = t.Ticket.Id,
                        SelectedTicket = t.Ticket,
                        UserOrder = orderItem,
                        Quantity = t.Quantity
                    }).ToList();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Your order is completed. The order contains: ");

                var totalPrice = 0;

                for (int i = 1; i <= result.Count(); i++) {
                    var item = result[i - 1];
                    totalPrice += item.Quantity + item.SelectedTicket.Price;
                    sb.AppendLine(i.ToString() + ". " + item.SelectedTicket.Movie + " with price of: " + item.SelectedTicket.Price + " and quantity of: " + item.Quantity);
                }

                sb.AppendLine("Total Price: " + totalPrice.ToString());

                message.Content = sb.ToString();

                ticketInOrders.AddRange(result);

                foreach (var ticket in ticketInOrders)
                {
                    this._ticketInOrderRepository.Insert(ticket);
                }

                loggedInUser.UserCart.TicketInShoppingCart.Clear();

                this._emailMessageRepository.Insert(message);

                this._userRepository.Update(loggedInUser);

                return true;
            }

            return false;
        }
    }
}
