 using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketShop.Repository.Interface;
using TicketShop.Domain.Identity;

namespace TicketShop.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<TicketShopApplicationUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<TicketShopApplicationUser>();
        }
        public IEnumerable<TicketShopApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public TicketShopApplicationUser Get(string id)
        {
            return entities
                .Include(t => t.UserCart)
                .Include("UserCart.TicketInShoppingCart")
                .Include("UserCart.TicketInShoppingCart.Ticket")
                .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(TicketShopApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(TicketShopApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(TicketShopApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
