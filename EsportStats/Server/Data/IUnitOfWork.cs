using EsportStats.Server.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ITopListEntryRepository TopListEntries { get; }
        IUserRepository Users { get; }
        IExternalUserRepository ExternalUsers { get; }

        int SaveChanges();
    }
}
