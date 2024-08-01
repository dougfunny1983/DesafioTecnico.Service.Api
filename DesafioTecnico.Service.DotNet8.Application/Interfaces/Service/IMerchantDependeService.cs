using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Interfaces.Service
{
    public interface IMerchantDependeService
    {
        Task<bool> ValidateMerchantAsync(string merchant);
    }
}
