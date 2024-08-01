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
    public sealed class MerchantDependeRepository : IMerchantDependeRepository
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly ILogger _logger;

        public MerchantDependeRepository(IDatabaseContextFactory databaseContextFactory, ILogger logger)
        {
            _databaseContextFactory = databaseContextFactory;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todas as transações.
        /// </summary>
        public async Task<IEnumerable<MerchantDependentModel>> GetAllTransactionsAsync()
        {
            try
            {
                _logger.Information("Getting all transactions.");
                var context = _databaseContextFactory.FactoryContext(EDataBase.MerchantDependent);
                var result = await context.RunGetAllAsync<MerchantDependentModel>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return default;
            }
        }

        /// <summary>
        /// Obtém transações pelo ID.
        /// </summary>
        public async Task<IEnumerable<MerchantDependentModel>> GetTransactionsAsync(int id)
        {
            try
            {
                _logger.Information($"Getting transactions with id {id}.");
                var context = _databaseContextFactory.FactoryContext(EDataBase.MerchantDependent);
                var result = await context.RunGetAsync<MerchantDependentModel>(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return default;
            }
        }

        /// <summary>
        /// Salva uma transação.
        /// </summary>
        public async Task<bool> SaveTransactionAsync(MerchantDependentModel entity)
        {
            try
            {
                _logger.Information("Saving transaction.");
                var context = _databaseContextFactory.FactoryContext(EDataBase.MerchantDependent);
                await context.RunUpdateAsync(entity);
                _logger.Information("Transaction saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return false;
            }
        }
    }
}
