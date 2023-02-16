using System;
using System.Collections;
using System.Collections.Generic;
using TicketShop.Domain.Identity;

namespace TicketShop.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public TicketShopApplicationUser User { get; set; }
        public ICollection<TicketInOrder> Tickets { get; set; } // ova beshe IEnumerable
    }
}
