using DesafioTecnico.Service.DotNet8.Application.Interfaces.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Services
{
    public sealed class AccountSemaphoreService : IAccountSemaphoreService
    {
        private readonly ConcurrentDictionary<int, SemaphoreSlim> _accountLocks = new();

        public SemaphoreSlim GetLockForAccount(int accountId) =>
            _accountLocks.GetOrAdd(accountId, _ => new SemaphoreSlim(1, 1));

        public void ReleaseLockForAccount(int accountId) =>
            _accountLocks.TryRemove(accountId, out _);
    }
}
