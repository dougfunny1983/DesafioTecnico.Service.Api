using DesafioTecnico.Service.DotNet8.Application.Interfaces.Patterns.ChainOfResponsibility;
using DesafioTecnico.Service.DotNet8.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Patterns.ChainOfResponsibility
{
    public sealed class FoodBalanceHandler : IFoodBalanceHandler
    {
        private IBalanceHandler _nextHandler;

        public void SetNext(IBalanceHandler handler)
        {
            _nextHandler = handler;
        }

        public bool Handle(ClientModel client, decimal amount)
        {
            if (client.Balances.FOOD >= amount)
            {
                client.Balances.FOOD -= amount;
                client.UpdateTotalAmount(amount);
                return true;
            }
            else
            {
                var remainingAmount = amount - client.Balances.FOOD;
                client.UpdateTotalAmount(client.Balances.FOOD);
                client.Balances.FOOD = 0;
                return _nextHandler?.Handle(client, remainingAmount) ?? false;
            }
        }
    }
}
