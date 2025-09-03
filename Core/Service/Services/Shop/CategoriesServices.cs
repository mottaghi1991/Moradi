using Core.Service.Interface.Shop;
using Data.MasterInterface;
using Domain.DrShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Shop
{
    public class CategoriesServices : ICategory
    {
        private readonly IMaster<Category> _master;

        public CategoriesServices(IMaster<Category> master)
        {
            _master = master;
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
