using DesafioTecnico.Service.DotNet8.Application.Configurations;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.DatabaseContext.Colections;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.Infra;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.Patterns.ChainOfResponsibility;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.Service;
using DesafioTecnico.Service.DotNet8.Application.Patterns.ChainOfResponsibility;
using DesafioTecnico.Service.DotNet8.Application.Services;
using DesafioTecnico.Service.DotNet8.Infrastructure.Repository;
using DesafioTecnico.Service.DotNet8.InfrastructureCore.Context;
using DesafioTecnico.Service.DotNet8.InfrastructureCore.Factory;
using DesafioTecnico.Service.DotNet8.InfrastructureCore.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.InfrastructureCore.InversionOfControl
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolver
    {
        public static IServiceCollection AddDependencyResolver(this IServiceCollection services)
        {
            AddLogging(services);
            AddServices(services);
            AddRepositories(services);
            AddDatabaseContexts(services);
            return services;
        }

        #region [Logging]

        private static void AddLogging(IServiceCollection services)
        {
            Log.Logger = LoggerConfigurator.ConfigureLogger();
            services.AddSingleton(Log.Logger);
        }

        #endregion [Logging]

        #region [Services]

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddTransient<IFoodBalanceHandler, FoodBalanceHandler>();
            services.AddTransient<IMealBalanceHandler, MealBalanceHandler>();
            services.AddTransient<ICashBalanceHandler, CashBalanceHandler>();
            services.AddScoped<IMerchantDependeService, MerchantDependeService>();
            services.AddScoped<IAccountSemaphoreService, AccountSemaphoreService>();
        }

        #endregion [Services]

        #region [Repositories]

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientsRepository, ClientsRepository>();
            services.AddScoped<IMerchantDependeRepository, MerchantDependeRepository>();
        }

        #endregion [Repositories]

        #region [Database Contexts]

        private static void AddDatabaseContexts(this IServiceCollection services)
        {
            var filePathClient = Configurations.PathClients;
            var filePathMerchantDependent = Configurations.PathMerchantDepende;
            services.AddSingleton<IClientsContext>(provider => new ClientsContext(filePathClient));
            services.AddSingleton<IMerchantDependeContext>(provider => new MerchantDependeContext(filePathMerchantDependent));
            services.AddSingleton<IDatabaseContextFactory, DatabaseContextFactory>();
        }

        #endregion [Database Contexts]
    }
}
