using DesafioTecnico.Service.DotNet8.Application.Enuns;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.DatabaseContext.Base;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.DatabaseContext.Colections;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.InfrastructureCore.Factory
{
    public sealed class DatabaseContextFactory : IDatabaseContextFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private const string MENSAGE_ERRO = "Nome de Configuração invalida: {0}";

        public DatabaseContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IBaseContext FactoryContext(EDataBase dataBase) =>
            dataBase switch
            {
                EDataBase.MerchantDependent => _serviceProvider.GetRequiredService<IMerchantDependeContext>(),
                EDataBase.Client => _serviceProvider.GetRequiredService<IClientsContext>(),
                _ => throw new ArgumentException(string.Format(MENSAGE_ERRO, dataBase))
            };
    }
}
