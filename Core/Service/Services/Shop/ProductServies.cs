using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Dto.ViewModel.Store.ProductDto;
using Core.Interface.Store;

using Data.MasterInterface;
using Domain.Shop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services.Store
{
    public class ProductServies : IProduct
    {
        private readonly IMaster<Product> _master;
        private readonly IMaster<ProductImage> _masterImage;
        private readonly IMapper _mapper;

        public ProductServies(IMaster<Product> master, IMaster<ProductImage> masterImage, IMapper mapper)
        {
            _master = master;
            _masterImage = masterImage;
            _mapper = mapper;
        }

        public async Task<bool> DeleteImage(int productImageId)
        {
            var obj=await GetProductImageById(productImageId);
            return await _masterImage.DeleteAsync(obj);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
           return await _master.GetAllEfAsync();
        }

        public async Task<IEnumerable<ProductImage>> GetAllImageOfProductById(int ProductId)
        {
            return await _masterImage.GetAllEfAsync(a => a.ProductId == ProductId);
        }

        public Task<IEnumerable<Product>> GetProductBybcategory(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetProductById(int ProductId)
        {
            var obj = await _master.GetAllAsQueryable().Include(a => a.Category).ToListAsync();
           return obj.FirstOrDefault(a => a.Id == ProductId);
        }

        public async Task<IEnumerable<Product>> GetProductBySubcategory(int CategoryId)
        {
            return await _master.GetAllEfAsync(a => a.CategoryId == CategoryId);
        }

      

        public async Task<ProductImage> GetProductImageById(int productImageId)
        {
            var obj = await _masterImage.GetAllEfAsync(a => a.Id == productImageId);
            return obj.FirstOrDefault();
        }

        public ShowProductDetailVm GetShowProductDetailVmByProductId(int ProductId)
        {
            var product = _master.GetAllAsQueryable()
                          .Where(p => p.Id == ProductId)
                  .ProjectTo<ShowProductDetailVm>(_mapper.ConfigurationProvider).FirstOrDefault();
                  
            return product;
        }

        public async Task<Product> Insert(Product product)
        {
            return await _master.InsertAsync(product);
        }

        public async Task<ProductImage> InsertImage(ProductImage productImage)
        {
           return await _masterImage.InsertAsync(productImage);
        }



        public async Task<Product> Update(Product product)
        {
       return await _master.UpdateAsync(product);
        }

      
     

    }
}
