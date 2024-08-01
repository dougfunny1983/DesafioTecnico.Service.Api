using DesafioTecnico.Service.DotNet8.Application.Interfaces.DatabaseContext.Colections;
using DesafioTecnico.Service.DotNet8.InfrastructureCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.InfrastructureCore.Context
{
    public sealed class MerchantDependeContext : IMerchantDependeContext
    {
        private readonly string _filePathMerchantDependeContext;

        public MerchantDependeContext(string filePathMerchantDependeContext)
        {
            _filePathMerchantDependeContext = filePathMerchantDependeContext;
        }

        public async Task<IEnumerable<T>> RunGetAllAsync<T>()
        {
            return await JsonFileReader.ReadJsonFileAsync<T>(_filePathMerchantDependeContext);
        }

        public async Task<IEnumerable<T>> RunGetAsync<T>(int id)
        {
            var merchants = await JsonFileReader.ReadJsonFileAsync<T>(_filePathMerchantDependeContext);
            var result = merchants.Where(m => ((dynamic)m).Id == id);
            return result;
        }

        public async Task RunUpdateAsync<T>(T entity)
        {
            var merchants = await JsonFileReader.ReadJsonFileAsListAsync<T>(_filePathMerchantDependeContext);

            var merchantToUpdate = merchants.FirstOrDefault(m => ((dynamic)m).Id == ((dynamic)entity).Id);

            if (merchantToUpdate is not null)
            {
                merchants.Remove(merchantToUpdate);
                merchants.Add(entity);

                var jsonData = JsonSerializer.Serialize(merchants);

                await File.WriteAllTextAsync(_filePathMerchantDependeContext, jsonData);
            }
            else
            {
                throw new Exception("Merchant not found");
            }
        }

        public async Task RunSaveAllAsync<T>(IEnumerable<T> entities)
        {
            var jsonData = JsonSerializer.Serialize(entities);
            await File.WriteAllTextAsync(_filePathMerchantDependeContext, jsonData);
        }
    }
}
