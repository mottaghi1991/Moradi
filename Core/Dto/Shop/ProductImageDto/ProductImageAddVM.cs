using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Store.ProductImageDto
{
    public class ProductImageListVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<ProductImage> productImages  { get; set; }
    }
}
