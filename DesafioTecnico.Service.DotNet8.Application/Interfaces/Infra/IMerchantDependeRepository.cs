using DesafioTecnico.Service.DotNet8.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Interfaces.Infra
{
    public interface IMerchantDependeRepository
    {
        Task<IEnumerable<MerchantDependentModel>> GetTransactionsAsync(int id);

        Task<IEnumerable<MerchantDependentModel>> GetAllTransactionsAsync();

        Task<bool> SaveTransactionAsync(MerchantDependentModel entity);
    }
}
