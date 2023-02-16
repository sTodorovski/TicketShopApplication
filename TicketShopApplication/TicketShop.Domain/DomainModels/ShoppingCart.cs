using System;
using System.Collections;
using System.Collections.Generic;
using TicketShop.Domain.Identity;

namespace TicketShop.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public TicketShopApplicationUser Owner { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCart { get; set; }
    }
}
