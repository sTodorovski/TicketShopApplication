using System;
using TicketShop.Domain.DomainModels;

namespace TicketShop.Domain.Identity
{
    public class AddToShoppingCartDto
    {
        public Ticket SelectedTicket { get; set; }
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
