using Data.Migrations;
using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Dr
{
    public interface IPost
    {
        Task<Post> GetpostById(int id);
        Task<IEnumerable<Post>> GetAllPostPaging(int pageid, int number);
        Task<IEnumerable<Post>> GetTopPost(int number);
        Task<int> PostCount();
    }
}
