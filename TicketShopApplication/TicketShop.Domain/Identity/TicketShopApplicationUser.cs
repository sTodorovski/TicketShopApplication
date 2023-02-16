using Microsoft.AspNetCore.Identity;
using TicketShop.Domain.DomainModels;

namespace TicketShop.Domain.Identity
{
    public class TicketShopApplicationUser : IdentityUser
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public virtual ShoppingCart UserCart { get; set; } // pod ova
        public string NormalEmail { get; set; }
        public string Mejl { get; set; }
        public string NormalUsername { get; set; }
        public string Korisnik { get; set; }
    }
}
