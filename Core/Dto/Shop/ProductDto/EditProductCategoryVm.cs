using Data.Migrations;
using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Shop.ProductDto
{
    public class EditProductCategoryVm
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<ProductCategory> SelectCategory{ get; set; }
    }
}
