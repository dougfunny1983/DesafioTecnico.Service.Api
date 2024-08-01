using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Interfaces.Service
{
    public interface IAccountSemaphoreService
    {
        SemaphoreSlim GetLockForAccount(int accountId);

        void ReleaseLockForAccount(int accountId);
    }
}
