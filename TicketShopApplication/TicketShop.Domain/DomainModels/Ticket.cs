using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketShop.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {

        [Required]
        public string Movie { get; set; }
        [Required]
        public string Poster { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCart { get; set; }
        public virtual ICollection<TicketInOrder> Orders { get; set; }
    }
}
