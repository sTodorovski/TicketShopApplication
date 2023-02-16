using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.Identity;

namespace TicketShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<TicketShopApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<TicketInShoppingCart> TicketInShoppingCarts { get; set; }
        public virtual DbSet<TicketInOrder> TicketInOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ticket>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            //builder.Entity<TicketInShoppingCart>()
                //.HasKey(t => new { t.TicketId, t.ShoppingCartId });

            builder.Entity<TicketInShoppingCart>()
                .HasOne(t => t.Ticket)
                .WithMany(t => t.TicketInShoppingCart)
                .HasForeignKey(t => t.ShoppingCartId);

            builder.Entity<TicketInShoppingCart>()
                .HasOne(t => t.ShoppingCart)
                .WithMany(t => t.TicketInShoppingCart)
                .HasForeignKey(t => t.TicketId);

            builder.Entity<ShoppingCart>()
                .HasOne<TicketShopApplicationUser>(t => t.Owner)
                .WithOne(t => t.UserCart)
                .HasForeignKey<ShoppingCart>(t => t.OwnerId);

            //builder.Entity<TicketInOrder>()
                //.HasKey(t => new { t.TicketId, t.OrderId });

            builder.Entity<TicketInOrder>()
                .HasOne(t => t.SelectedTicket)
                .WithMany(t => t.Orders)
                .HasForeignKey(t => t.TicketId);

            builder.Entity<TicketInOrder>()
                .HasOne(t => t.UserOrder)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.OrderId);
        }
    }
}
