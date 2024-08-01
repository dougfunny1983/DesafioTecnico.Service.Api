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
    public sealed class ClientsContext : IClientsContext
    {
        private readonly string _filePathClient;

        public ClientsContext(string filePathClient)
        {
            _filePathClient = filePathClient;
        }

        public async Task<IEnumerable<T>> RunGetAllAsync<T>()
        {
            return await JsonFileReader.ReadJsonFileAsync<T>(_filePathClient);
        }

        public async Task<IEnumerable<T>> RunGetAsync<T>(int id)
        {
            var clients = await JsonFileReader.ReadJsonFileAsync<T>(_filePathClient);
            var result = clients.Where(c => ((dynamic)c).AccountId == id);
            return result;
        }

        public async Task RunUpdateAsync<T>(T entity)
        {
            var clients = await JsonFileReader.ReadJsonFileAsListAsync<T>(_filePathClient);

            var clientToUpdate = clients.FirstOrDefault(c => ((dynamic)c).AccountId == ((dynamic)entity).AccountId);

            if (clientToUpdate is not null)
            {
                clients.Remove(clientToUpdate);
                clients.Add(entity);

                var jsonData = JsonSerializer.Serialize(clients);

                await File.WriteAllTextAsync(_filePathClient, jsonData);
            }
            else
            {
                throw new Exception("Client not found");
            }
        }

        public async Task RunSaveAllAsync<T>(IEnumerable<T> entities)
        {
            var jsonData = JsonSerializer.Serialize(entities);
            await File.WriteAllTextAsync(_filePathClient, jsonData);
        }
    }
}
