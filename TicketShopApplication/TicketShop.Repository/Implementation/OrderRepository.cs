using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Repository.Interface;

namespace TicketShop.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }

        public List<Order> GetAllOrders()
        {
            return entities
                .Include(t => t.User)
                .Include(t => t.Tickets)
                .Include("Tickets.SelectedTicket")
                .ToListAsync().Result;
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return entities
                .Include(t => t.User)
                .Include(t => t.Tickets)
                .Include("Tickets.SelectedTicket")
                .SingleOrDefaultAsync(t => t.Id == model.Id).Result;
        }
    }
}
