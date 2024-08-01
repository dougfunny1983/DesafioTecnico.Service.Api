using DesafioTecnico.Service.DotNet8.Application.Enuns;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.DatabaseContext.Colections;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.Infra;
using DesafioTecnico.Service.DotNet8.Application.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Infrastructure.Repository
{
    public sealed class ClientsRepository : IClientsRepository
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly ILogger _logger;

        public ClientsRepository(IDatabaseContextFactory databaseContextFactory, ILogger logger)
        {
            _databaseContextFactory = databaseContextFactory;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os clientes.
        /// </summary>
        public async Task<IEnumerable<ClientModel>> GetAllClientsAsync()
        {
            try
            {
                _logger.Information("Getting all clients.");
                var context = _databaseContextFactory.FactoryContext(EDataBase.Client);
                var result = await context.RunGetAllAsync<ClientModel>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return default;
            }
        }

        /// <summary>
        /// Obtém um cliente pelo ID.
        /// </summary>
        public async Task<ClientModel> GetClientByIdAsync(int id)
        {
            try
            {
                _logger.Information($"Getting client with id {id}.");
                var context = _databaseContextFactory.FactoryContext(EDataBase.Client);
                var result = await context.RunGetAsync<ClientModel>(id);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return default;
            }
        }

        /// <summary>
        /// Salva um cliente.
        /// </summary>
        public async Task<bool> SaveClientAsync(ClientModel client)
        {
            try
            {
                _logger.Information("Saving client.");
                var context = _databaseContextFactory.FactoryContext(EDataBase.Client);
                await context.RunUpdateAsync(client);
                _logger.Information("Client saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Salva todos os clientes.
        /// </summary>
        public async Task SaveAllClientsAsync(IEnumerable<ClientModel> clients)
        {
            try
            {
                _logger.Information("Saving clients.");
                var context = _databaseContextFactory.FactoryContext(EDataBase.Client);
                await context.RunUpdateAsync(clients);
                _logger.Information("clients saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }
    }
}
