using DesafioTecnico.Service.DotNet8.Application.Enuns;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.Infra;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.Patterns.ChainOfResponsibility;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.Service;
using DesafioTecnico.Service.DotNet8.Application.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Services
{
    /// <summary>
    /// Serviço responsável por processar transações de clientes.
    /// </summary>
    public sealed class TransactionService : ITransactionService
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IFoodBalanceHandler _foodBalanceHandler;
        private readonly IMealBalanceHandler _mealBalanceHandler;
        private readonly ICashBalanceHandler _cashBalanceHandler;
        private readonly IAccountSemaphoreService _accountSemaphoreService;
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor da classe TransactionService.
        /// </summary>
        /// <param name="clientsRepository">Repositório de clientes.</param>
        /// <param name="foodBalanceHandler">Manipulador de saldo de FOOD.</param>
        /// <param name="mealBalanceHandler">Manipulador de saldo de MEAL.</param>
        /// <param name="cashBalanceHandler">Manipulador de saldo de CASH.</param>
        /// <param name="accountSemaphoreService">Serviço de bloqueio de conta.</param>
        /// <param name="logger">Serviço de logging.</param>
        public TransactionService(
            IClientsRepository clientsRepository,
            IFoodBalanceHandler foodBalanceHandler,
            IMealBalanceHandler mealBalanceHandler,
            ICashBalanceHandler cashBalanceHandler,
            IAccountSemaphoreService accountSemaphoreService,
            ILogger logger)
        {
            _clientsRepository = clientsRepository;
            _foodBalanceHandler = foodBalanceHandler;
            _mealBalanceHandler = mealBalanceHandler;
            _cashBalanceHandler = cashBalanceHandler;
            _accountSemaphoreService = accountSemaphoreService;
            _logger = logger;

            // Configura a cadeia de responsabilidade
            ConfigureBalanceHandlers();
        }

        /// <summary>
        /// Configura a cadeia de responsabilidade para os manipuladores de saldo.
        /// </summary>
        private void ConfigureBalanceHandlers()
        {
            _foodBalanceHandler.SetNext(_mealBalanceHandler);
            _mealBalanceHandler.SetNext(_cashBalanceHandler);
        }

        /// <summary>
        /// Processa uma transação.
        /// </summary>
        /// <param name="transaction">Modelo da transação.</param>
        /// <param name="cancellation">Token de cancelamento.</param>
        /// <returns>Modelo de código de resposta da transação.</returns>
        public async Task<TransactionResponseCodeModel> ProcessTransactionAsync(TransactionModel transaction, CancellationToken cancellation)
        {
            // Obtém o cliente pelo ID da conta
            var client = await GetClientAsync(transaction.Account);

            // Verifica se o cliente foi encontrado
            if (client is null)
            {
                return new TransactionResponseCodeModel(ETransactionResponseCode.OtherError);
            }

            // Verifica se o cliente tem fundos suficientes
            if (!HasSufficientFunds(client, transaction.TotalAmount))
            {
                return new TransactionResponseCodeModel(ETransactionResponseCode.InsufficientFunds);
            }

            var accountLock = _accountSemaphoreService.GetLockForAccount(transaction.Account);

            var somenteParaTeste = TimeSpan.FromMinutes(20);

            await accountLock.WaitAsync(somenteParaTeste, cancellation);

            try
            {
                return await ProcessTransactionWithLockAsync(transaction, client);
            }
            finally
            {
                accountLock.Release();
                _accountSemaphoreService.ReleaseLockForAccount(transaction.Account);
            }
        }

        /// <summary>
        /// Processa a transação com o bloqueio da conta.
        /// </summary>
        /// <param name="transaction">Modelo da transação.</param>
        /// <param name="client">Modelo do cliente.</param>
        /// <returns>Modelo de código de resposta da transação.</returns>
        private async Task<TransactionResponseCodeModel> ProcessTransactionWithLockAsync(TransactionModel transaction, ClientModel client)
        {
            // Armazena o estado original dos clientes para possível restauração em caso de falha
            var clients = await _clientsRepository.GetAllClientsAsync();

            try
            {
                // Deduz o saldo usando a cadeia de responsabilidade
                bool hasBalance = HandleBalance(client, transaction.TotalAmount);
                if (!hasBalance)
                {
                    return new TransactionResponseCodeModel(ETransactionResponseCode.InsufficientFunds);
                }

                // Tenta salvar o cliente atualizado no repositório
                bool saved = await SaveClientAsync(client);
                if (!saved)
                {
                    // Restaura o estado original do cliente em caso de falha
                    await RestoreOriginalClientsAsync(clients);
                    return new TransactionResponseCodeModel(ETransactionResponseCode.OtherError);
                }

                // Retorna o código de resposta de transação aprovada
                return new TransactionResponseCodeModel(ETransactionResponseCode.Approved);
            }
            catch (Exception ex)
            {
                // Log de erro
                _logger.Error(ex, "Erro ao processar a transação para a conta {AccountId}", transaction.Account);
                // Restaura o estado original do cliente em caso de falha
                await RestoreOriginalClientsAsync(clients);
                return new TransactionResponseCodeModel(ETransactionResponseCode.OtherError);
            }
        }

        /// <summary>
        /// Manipula o saldo do cliente.
        /// </summary>
        /// <param name="client">Modelo do cliente.</param>
        /// <param name="amount">Quantia a ser deduzida.</param>
        /// <returns>Verdadeiro se o saldo for suficiente, caso contrário, falso.</returns>
        private bool HandleBalance(ClientModel client, decimal amount)
        {
            return _foodBalanceHandler.Handle(client, amount);
        }

        /// <summary>
        /// Restaura o estado original dos clientes.
        /// </summary>
        /// <param name="clients">Lista de clientes a serem restaurados.</param>
        private async Task RestoreOriginalClientsAsync(IEnumerable<ClientModel> clients)
        {
            _logger.Information("Restaurando o estado original dos clientes.");
            await _clientsRepository.SaveAllClientsAsync(clients);
        }

        /// <summary>
        /// Obtém o cliente pelo ID da conta.
        /// </summary>
        /// <param name="accountId">ID da conta.</param>
        /// <returns>Modelo do cliente.</returns>
        private async Task<ClientModel> GetClientAsync(int accountId)
        {
            return await _clientsRepository.GetClientByIdAsync(accountId);
        }

        /// <summary>
        /// Verifica se o cliente tem fundos suficientes.
        /// </summary>
        /// <param name="client">Modelo do cliente.</param>
        /// <param name="amount">Quantia a ser verificada.</param>
        /// <returns>Verdadeiro se o cliente tiver fundos suficientes, caso contrário, falso.</returns>
        private static bool HasSufficientFunds(ClientModel client, decimal amount)
        {
            return client.TotalAmount >= amount;
        }

        /// <summary>
        /// Salva o cliente atualizado no repositório.
        /// </summary>
        /// <param name="client">Modelo do cliente.</param>
        /// <returns>Verdadeiro se o cliente foi salvo com sucesso, caso contrário, falso.</returns>
        private async Task<bool> SaveClientAsync(ClientModel client)
        {
            return await _clientsRepository.SaveClientAsync(client);
        }
    }
}
