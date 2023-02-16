using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.Identity;

namespace TicketShop.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<TicketShopApplicationUser> GetAll();
        TicketShopApplicationUser Get(string id);
        void Insert(TicketShopApplicationUser entity);
        void Update(TicketShopApplicationUser entity);
        void Delete(TicketShopApplicationUser entity);
    }
}
