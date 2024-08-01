using DesafioTecnico.Service.DotNet8.Api.Models;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.Service;
using DesafioTecnico.Service.DotNet8.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DesafioTecnico.Service.DotNet8.Api.Controllers
{
    /// <summary>
    /// Controlador para gerenciar transações.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMerchantDependeService _merchantDependeService;

        /// <summary>
        /// Construtor do controlador de transações.
        /// </summary>
        /// <param name="transactionService">Serviço de transações.</param>
        /// <param name="merchantDependeService">Serviço de validação de merchants.</param>
        public TransactionController(ITransactionService transactionService, IMerchantDependeService merchantDependeService)
        {
            _transactionService = transactionService;
            _merchantDependeService = merchantDependeService;
        }

        /// <summary>
        /// Processa uma transação.
        /// </summary>
        /// <param name="payload">Dados da transação.</param>
        /// <param name="cancellation">Token de cancelamento.</param>
        /// <returns>Resultado do processamento da transação.</returns>
        [HttpPost("process")]
        [ProducesResponseType(typeof(TransactionResponseCodeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> ProcessTransaction([FromBody][Required] TransactionRequestModel payload, CancellationToken cancellation)
        {
            bool isMerchantEscapeValid = await _merchantDependeService.ValidateMerchantAsync(payload.Merchant);

            TransactionModel transaction = new(isMerchantEscapeValid, payload.Account, payload.TotalAmount, payload.Mcc, payload.Merchant);
            var result = await _transactionService.ProcessTransactionAsync(transaction, cancellation);

            return Ok(result);
        }
    }
}
