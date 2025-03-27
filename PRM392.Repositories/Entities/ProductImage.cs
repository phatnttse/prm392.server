using PRM392.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories.Entities
{
    public class ProductImage : BaseEntity
    {
        public string? ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public string? ImageUrl { get; set; }
    }
}
