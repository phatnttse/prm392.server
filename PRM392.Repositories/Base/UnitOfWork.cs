using Microsoft.AspNetCore.Identity;
using PRM392.Repositories.DbContext;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UserAccountRepository UserAccountRepository { get; }
        public UserRoleRepository UserRoleRepository { get; }

        public UnitOfWork(ApplicationDbContext context,
            UserAccountRepository userAccountRepository,
            UserRoleRepository userRoleRepository
        )
        {
            _context = context;
            UserAccountRepository = userAccountRepository;
            UserRoleRepository = userRoleRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
