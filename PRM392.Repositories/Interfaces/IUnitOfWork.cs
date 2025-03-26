using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        UserAccountRepository UserAccountRepository { get; }
        UserRoleRepository UserRoleRepository { get; }
    }

}
