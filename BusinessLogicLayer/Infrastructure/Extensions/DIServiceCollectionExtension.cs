using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BllServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IUnitOfWork, UnitOfWork>(uow => new UnitOfWork(connectionString));
            return services;
        }
    }
}
