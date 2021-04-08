using EsportStats.Server.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ITopListEntryRepository TopListEntries { get; private set; }
        public IUserRepository Users { get; private set; }
        public IExternalUserRepository ExternalUsers { get; private set; }
        public IHeroStatRepository HeroStats { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            TopListEntries = new TopListEntryRepository(_context);
            Users = new UserRepository(_context);
            ExternalUsers = new ExternalUserRepository(_context);
            HeroStats = new HeroStatRepository(_context);
        }

        public int SaveChanges() //TODO: Async?
        {
            return _context.SaveChanges();
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
