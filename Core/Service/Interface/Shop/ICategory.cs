

using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Shop
{
    public interface ICategory
    {
        Task<IEnumerable<Category>> GetAllByActive(bool active);
        public Task<IEnumerable<Category>> GetAllCategory();
     
        public Task<Category> Insert(Category Category);
        public Task<Category> Update(Category Category);
        public Task<bool> Delete(int QuestionId);
        public Task<Category> GetCategoryById(int CategoryId);
    }
}
