using EBilets.Domain.DomainModels;
using EBilets.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EBilets.Reposiotory
{
    public class ApplicationDbContext : IdentityDbContext<EBiletsUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Billet> Billets { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //primarni kluchevi

            builder.Entity<Billet>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<BilletInShoppingCart>()
                .HasKey(z => new { z.BilletId, z.ShoppingCartId });

            //ManyToMany 
            builder.Entity<BilletInShoppingCart>()
                .HasOne(z => z.CurrentBillet)
                .WithMany(z => z.BilletInShoppingCart)
                .HasForeignKey(z => z.BilletId);

            builder.Entity<BilletInShoppingCart>()
               .HasOne(z => z.UserCart) // shto ima vo pogorniot entitet
               .WithMany(z => z.BilletInShoppingCart) //vo kakva vrska e soodvetnoto svojstvo od negoviot model
               .HasForeignKey(z => z.ShoppingCartId); //shto e nadvoreshniot kluch vo entitetot

            //OneToOne

            builder.Entity<ShoppingCart>()
                .HasOne<EBiletsUser>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);

            //ManyToMany

            builder.Entity<BilletInOrders>()
              .HasKey(z => new { z.BilletId, z.OrderId });

            builder.Entity<BilletInOrders>()
                .HasOne(z => z.Billet)
                .WithMany(z => z.BilletInOrders)
                .HasForeignKey(z => z.BilletId);

            builder.Entity<BilletInOrders>()
                .HasOne(z => z.Order)
                .WithMany(z => z.BilletsInOrder)
                .HasForeignKey(z => z.OrderId);
        }




    }
}
