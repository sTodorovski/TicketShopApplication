using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;
using TicketShop.Domain.Identity;
using TicketShop.Repository;
using TicketShop.Services.Interface;

namespace TicketShopApplication.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(this._shoppingCartService.getShoppingCartInfo(userId));
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var CustomerService = new CustomerService();

            var ChargeService = new ChargeService();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this._shoppingCartService.getShoppingCartInfo(userId);

            var customer = CustomerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken,
            });

            var charge = ChargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "KinoMania Payment",
                Currency = "mkd",
                Customer = customer.Id
            });

            if(charge.Status == "succeeded")
            {
                var result = this.OrderNow();

                if(result)
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
                else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }

            return RedirectToAction("Index", "ShoppingCart");
        }

        public IActionResult DeleteFromShoppingCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.deleteTicketFromShoppingCart(userId, id);

            if(result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            } 
            else
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
        }

        private Boolean OrderNow()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.orderNow(userId);

            return result;
        }
    }
}
