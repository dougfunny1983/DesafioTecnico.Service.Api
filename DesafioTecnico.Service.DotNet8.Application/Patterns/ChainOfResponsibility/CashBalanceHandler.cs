using DesafioTecnico.Service.DotNet8.Application.Interfaces.Patterns.ChainOfResponsibility;
using DesafioTecnico.Service.DotNet8.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Patterns.ChainOfResponsibility
{
    public sealed class CashBalanceHandler : ICashBalanceHandler
    {
        private IBalanceHandler _nextHandler;

        public void SetNext(IBalanceHandler handler)
        {
            _nextHandler = handler;
        }

        public bool Handle(ClientModel client, decimal amount)
        {
            if (client.Balances.CASH >= amount)
            {
                client.Balances.CASH -= amount;
                client.UpdateTotalAmount(amount);
                return true;
            }
            else
            {
                // Saldo insuficiente em CASH
                return false;
            }
        }
    }
}
