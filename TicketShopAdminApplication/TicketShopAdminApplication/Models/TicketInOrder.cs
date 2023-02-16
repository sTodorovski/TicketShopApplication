using System.Net.Sockets;
using System;

namespace TicketShopAdminApplication.Models
{
    public class TicketInOrder
    {
        public Guid TicketId { get; set; }
        public Ticket SelectedTicket { get; set; }
        public Guid OrderId { get; set; }
        public Order UserOrder { get; set; }
        public int Quantity { get; set; }
    }
}
