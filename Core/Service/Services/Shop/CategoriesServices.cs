using Core.Service.Interface.Shop;
using Data.MasterInterface;
using Domain.Shop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Service.Services.Shop
{
    public class CategoriesServices : ICategory
    {
        private readonly IMaster<Category> _master;
        private readonly IMaster<ProductCategory> _PCmaster;

        public CategoriesServices(IMaster<Category> master, IMaster<ProductCategory> pCmaster)
        {
            _master = master;
            _PCmaster = pCmaster;
        }

        public Task<bool> BulkDeletePC(List<ProductCategory> productCategories)
        {
           return _PCmaster.BulkeDeleteAsync(productCategories);
        }

        public Task<bool> BulkInsertPC(List<ProductCategory> productCategories)
        {
            return _PCmaster.BulkeInsertAsync(productCategories);
        }

        public async Task<bool> Delete(int QuestionId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllByActive(bool active)
        {
            return await _master.GetAllEfAsync(a => a.IsActive == active);
        }

        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            return await _master.GetAllEfAsync();
        }

    

        public async Task<Category> GetCategoryById(int CategoryId)
        {
            var obj = await _master.GetAllEfAsync(a => a.Id == CategoryId);
            return obj.FirstOrDefault();
        }

        public async Task<IEnumerable<ProductCategory>> GetCateoryOfProduct(int ProductId)
        {
            return _PCmaster.GetAllAsQueryable(a => a.ProductId == ProductId).Include(a => a.Product).Include(a => a.Category).ToList();
        }

        public async Task<Category> Insert(Category Category)
        {
            return await _master.InsertAsync(Category);
        }

        public async Task<Category> Update(Category Category)
        {
            return await _master.UpdateAsync(Category);
        }
    }
}
