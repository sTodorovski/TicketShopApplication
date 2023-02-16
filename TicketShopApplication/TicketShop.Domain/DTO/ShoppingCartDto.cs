using System.Collections.Generic;
using TicketShop.Domain.DomainModels;

namespace TicketShop.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<TicketInShoppingCart> TicketInShoppingCarts { get; set; }
        public int TotalPrice { get; set; }
    }
}
