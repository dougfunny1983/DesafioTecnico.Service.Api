using DesafioTecnico.Service.DotNet8.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Interfaces.Infra
{
    public interface IClientsRepository
    {
        /// <summary>
        /// Obtém um cliente pelo ID.
        /// </summary>
        Task<ClientModel> GetClientByIdAsync(int id);

        /// <summary>
        /// Obtém todos os clientes.
        /// </summary>
        Task<IEnumerable<ClientModel>> GetAllClientsAsync();

        /// <summary>
        /// Salva um cliente.
        /// </summary>
        Task<bool> SaveClientAsync(ClientModel client);

        /// <summary>
        /// Salva todos os cliente.
        /// </summary>
        Task SaveAllClientsAsync(IEnumerable<ClientModel> clients);
    }
}
