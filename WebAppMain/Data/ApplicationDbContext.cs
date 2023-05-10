using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppMain.Models;

namespace WebAppMain.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Halls> Halls { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Visiting> Visiting { get; set; }
        public DbSet<Visitors> Visitors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {



            base.OnModelCreating(builder);// Задает схему для базы данных
            builder.HasDefaultSchema("Identity");   /* Переименовывает таблицу пользователей из AspNetUsers в Identity.User. */
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });


            builder.Entity<Services>()
                 .HasMany(h => h.Visitings)
                 .WithOne(v => v.Services)
                 .HasForeignKey(v => v.ServicecId);


            builder.Entity<Visitors>()
                            .HasOne(v => v.Visiting)
                            .WithOne(v => v.Visitor)
                            .HasForeignKey<Visiting>(v => v.VisitorsId);

            builder.Entity<Halls>()
       .HasMany(h => h.Visitings)
       .WithOne(v => v.Halls)
       .HasForeignKey(v => v.HallsId)
       .OnDelete((DeleteBehavior)ReferentialAction.NoAction);

            builder.Entity<Visiting>()
            .HasIndex(v => new { v.HallsId, v.ComputerId, v.DateAndTimeOfTheVisit })
            .IsUnique();


            builder.Entity<Computer>()
                              .HasOne(c => c.Halls)
                              .WithMany(h => h.Computers)
                              .HasForeignKey(c => c.HallsId);

            builder.Entity<Visiting>()
                            .HasOne(v => v.ApplicationUser)
                            .WithMany(au => au.Visitings)
                            .HasForeignKey(v => v.ApplicationUserId);




        }
    }
}
