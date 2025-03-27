using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Category
{
    public class CreateUpdateCategoryDTO
    {
        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
