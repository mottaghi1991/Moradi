
using Core.Dto.ViewModel.Store.ProductDto;
using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Store
{
    public interface IProduct
    {
    
   
        public Task<IEnumerable<Product>> GetAll();
        public Task<IEnumerable<Product>> GetProductBybcategory(int CategoryId);
        public Task<Product> GetProductById(int ProductId);
        public Task<Product> Insert(Product product);
        public Task<Product> Update(Product product);
        public Task<IEnumerable<ProductImage>> GetAllImageOfProductById(int ProductId);
        public Task<ProductImage> InsertImage(ProductImage productImage);
        public Task<bool> DeleteImage(int productImageId);
        public Task<ProductImage> GetProductImageById(int productImageId);

    }
}
