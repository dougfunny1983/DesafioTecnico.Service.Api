using DesafioTecnico.Service.DotNet8.Application.Enuns;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.DatabaseContext.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Interfaces.DatabaseContext.Colections
{
    public interface IDatabaseContextFactory
    {
        public IBaseContext FactoryContext(EDataBase dataBase);
    }
}
