using EsportStats.Server.Data.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<TopListEntry> TopListEntries { get; set; }
        public DbSet<ExternalUser> ExternalUsers{ get; set; }
        public DbSet<HeroStat> HeroStats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()                
                .HasAlternateKey(u => u.SteamId);

            builder.Entity<ExternalUser>()
                .HasKey(u => u.SteamId);


            builder.Entity<TopListEntry>()                
                .HasOne(entry => entry.User)
                .WithMany(user => user.TopListEntries)
                .HasForeignKey(entry => entry.UserId); // FK is a string, which is nullable, so this navigation property is optional!

            builder.Entity<TopListEntry>()
                .HasOne(entry => entry.ExternalUser)
                .WithMany(user => user.TopListEntries)
                .HasForeignKey(entry => entry.ExternalUserId); // FK is a ulong?, which is nullable, so this navigation property is optional!

            // TODO: validate that a TopListEntry always has at least one of the two possible navigation properties (ApplicationUser or ExternalUser)
            // TODO: should the navigation properties be defined here for the HeroStat.SteamId towards both user types?

        }
    }
}
