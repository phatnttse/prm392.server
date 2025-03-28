using PRM392.Repositories.Entities;
using PRM392.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<ApplicationResponse> GetProfile();
    }
}
