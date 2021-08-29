using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Server.Persistence {
    public interface IPersistenceContext : IDbContextDependencies, IDbContextPoolable, IDbSetCache, IDisposable, IInfrastructure<IServiceProvider> {
        DbSet<TModel> Set<TModel>() where TModel : class;
    }
}