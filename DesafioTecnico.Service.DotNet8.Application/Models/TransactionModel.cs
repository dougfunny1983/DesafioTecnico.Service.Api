using DesafioTecnico.Service.DotNet8.Application.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Models
{
    /// <summary>
    /// Representa um modelo de transação contendo detalhes sobre uma transação financeira.
    /// </summary>
    public sealed class TransactionModel
    {
        /// <summary>
        /// Obtém ou define a conta associada à transação.
        /// </summary>
        public int Account { get; set; }

        /// <summary>
        /// Obtém ou define o valor total da transação.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Obtém ou define o Código de Categoria do Comerciante (MCC) da transação.
        /// </summary>
        public int Mcc { get; set; }

        /// <summary>
        /// Obtém ou define o comerciante associado à transação.
        /// </summary>
        public string Merchant { get; set; }

        /// <summary>
        /// Obtém o tipo de saldo determinado pelo MCC da transação.
        /// </summary>
        public EBalanceType BalanceType { get; private set; }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="TransactionModel"/>.
        /// </summary>
        /// <param name="account">A conta associada à transação.</param>
        /// <param name="totalAmount">O valor total da transação.</param>
        /// <param name="mcc">O Código de Categoria do Comerciante (MCC) da transação como string.</param>
        /// <param name="merchant">O comerciante associado à transação.</param>
        public TransactionModel(bool merchantEscape, string account, decimal totalAmount, string mcc, string merchant)
        {
            Account = int.Parse(account);
            TotalAmount = totalAmount;
            Mcc = int.Parse(mcc);
            Merchant = merchant;
            BalanceType = DetermineBalanceType(merchantEscape, Mcc);
        }

        /// <summary>
        /// Determina o tipo de saldo com base no MCC da transação e se o merchant é válido.
        /// </summary>
        /// <param name="merchantEscape">Indica se o merchant é válido.</param>
        /// <param name="mcc">O Código de Categoria do Comerciante (MCC) da transação.</param>
        /// <returns>O tipo de saldo determinado <see cref="BalanceType"/>.</returns>
        private static EBalanceType DetermineBalanceType(bool merchantEscape, int mcc)
        {
            if (merchantEscape)
            {
                return EBalanceType.CASH;
            }

            return mcc switch
            {
                5411 or 5412 => EBalanceType.FOOD,
                5811 or 5812 => EBalanceType.MEAL,
                _ => EBalanceType.CASH
            };
        }
    }
}
