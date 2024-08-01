using DesafioTecnico.Service.DotNet8.Application.Interfaces.Patterns.ChainOfResponsibility;
using DesafioTecnico.Service.DotNet8.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Patterns.ChainOfResponsibility
{
    public sealed class MealBalanceHandler : IMealBalanceHandler
    {
        private IBalanceHandler _nextHandler;

        public void SetNext(IBalanceHandler handler)
        {
            _nextHandler = handler;
        }

        public bool Handle(ClientModel client, decimal amount)
        {
            if (client.Balances.MEAL >= amount)
            {
                client.Balances.MEAL -= amount;
                client.UpdateTotalAmount(amount);
                return true;
            }
            else
            {
                var remainingAmount = amount - client.Balances.MEAL;
                client.UpdateTotalAmount(client.Balances.MEAL);
                client.Balances.MEAL = 0;
                return _nextHandler?.Handle(client, remainingAmount) ?? false;
            }
        }
    }
}
