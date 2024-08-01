using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Interfaces.DatabaseContext.Base
{
    public interface IBaseContext
    {
        Task<IEnumerable<T>> RunGetAsync<T>(int id);

        Task<IEnumerable<T>> RunGetAllAsync<T>();

        Task RunUpdateAsync<T>(T entity);

        Task RunSaveAllAsync<T>(IEnumerable<T> entities);
    }
}
