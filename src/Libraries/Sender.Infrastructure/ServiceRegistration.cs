using Microsoft.Extensions.DependencyInjection;
using Sender.Core.Interfaces;
using Sender.Infrastructure.Services.Cache;
using Sender.Infrastructure.Services.Tsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureRegistration(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddMemoryCache();

            serviceCollection.AddHttpClient();

            serviceCollection.AddHttpClient<TsoftClient>();

            serviceCollection.AddScoped<IMemoryService, MemoryService>();
        }
    }
}
