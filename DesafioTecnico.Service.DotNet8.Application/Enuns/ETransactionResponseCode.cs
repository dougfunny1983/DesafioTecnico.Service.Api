using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Enuns
{
    /// <summary>
    /// Enumeração para os códigos de resposta da transação.
    /// </summary>
    public enum ETransactionResponseCode
    {
        [Description("Transação aprovada")]
        Approved = 00,

        [Description("Transação rejeitada por saldo insuficiente.")]
        InsufficientFunds = 51,

        [Description("Outros problemas que impedem a transação de ser processada.")]
        OtherError = 07
    }
}
