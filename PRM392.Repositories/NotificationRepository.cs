using PRM392.Repositories.Base;
using PRM392.Repositories.DbContext;
using PRM392.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
