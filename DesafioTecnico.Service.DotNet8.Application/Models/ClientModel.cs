using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Models
{
    public sealed class ClientModel
    {
        [JsonPropertyName("accountId")]
        public int AccountId { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("balances")]
        public BalancesModel Balances { get; set; }

        /// <summary>
        /// Atualiza o valor total do cliente.
        /// </summary>
        public void UpdateTotalAmount(decimal amount)
        {
            TotalAmount -= amount;
        }
    }
}
