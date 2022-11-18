using Integrators.Abstractions;
using Integrators.Core;
using Integrators.Dispatcher;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic
{
    public class MoneyIntegrator: 合気道Integrator
    {
        protected override void Init(IServiceCollection services, IConfiguration configuration)
        {
            base.Init(services, configuration);
            services.AddSingleton(MoneyManagerFactory);
            services.AddSingleton(HostDispatcherFactory);
            services.AddSingleton(MemoryCacheFactory);
            services.AddHostedService(TransferBotFactory);
        }

        private MoneyManager MoneyManagerFactory(IServiceProvider provider)
        {
            var connectionString = Configuration!.GetSection("ConnectionString").Value ?? throw new ArgumentNullException("ConnectionString");
            var memCache = provider.GetRequiredService<MemoryCache>();
            return new MoneyManager(memCache, connectionString);
        }

        private MoneyBot TransferBotFactory(IServiceProvider provider)
        {
            var dispatcher = provider.GetRequiredService<MoneyDispatcher>();
            var token = Configuration?.GetSection("BotToken").Value ?? throw new ArgumentNullException("BotToken");
            var bot = new MoneyBot(token, dispatcher);
            return bot;
        }

        private MoneyDispatcher HostDispatcherFactory(IServiceProvider provider)
        {
            var dispatcher = new MoneyDispatcher(provider);
            return dispatcher;
        }

        private MemoryCache MemoryCacheFactory(IServiceProvider provider)
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            return cache;
        }
    }
}
