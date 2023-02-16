using System;
using System.ComponentModel.DataAnnotations;

namespace TicketShopAdminApplication.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
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
    }
}
