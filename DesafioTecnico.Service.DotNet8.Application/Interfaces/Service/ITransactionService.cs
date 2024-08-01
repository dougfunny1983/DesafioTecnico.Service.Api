using DesafioTecnico.Service.DotNet8.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Interfaces.Service
{
    public interface ITransactionService
    {
        Task<TransactionResponseCodeModel> ProcessTransactionAsync(TransactionModel transaction, CancellationToken cancellation);
    }
}
