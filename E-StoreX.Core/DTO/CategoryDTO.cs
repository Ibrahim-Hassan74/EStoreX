using Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreX.Core.DTO
{
    public record CategoryDTO(string Name, string Description)
    {
        public Category ToCategory()
        {
            return new Category
            {
                Name = Name,
                Description = Description
            };
        }
    }

    public record UpdateCategoryDTO(string Name, string Description, Guid Id);
}
