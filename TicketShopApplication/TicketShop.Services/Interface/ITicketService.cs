﻿using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.Identity;

namespace TicketShop.Services.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Ticket GetDetailsForTicket(Guid? id);
        void CreateNewTicket(Ticket t);
        void UpdateExistingTicket(Ticket t);
        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
        void DeleteTicket(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
    }
}
