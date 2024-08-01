using DesafioTecnico.Service.DotNet8.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Interfaces.Patterns.ChainOfResponsibility
{
    public interface IBalanceHandler
    {
        void SetNext(IBalanceHandler handler);

        bool Handle(ClientModel client, decimal amount);
    }
}
