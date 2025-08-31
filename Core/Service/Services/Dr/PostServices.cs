using Core.Service.Interface.Dr;
using Data.MasterInterface;
using Data.Migrations;
using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Dr
{
    public class PostServices : IPost
    {
        private readonly IMaster<Post> _master;

        public PostServices(IMaster<Post> master)
        {
            _master = master;
        }

        public async Task<IEnumerable<Post>> GetAllPostPaging(int pageid,int number)
        {
            var obj = await _master.GetPagingAsync(pageid, number);
            return obj.OrderByDescending(a=>a.Id);
        }

        public async Task<Post> GetpostById(int id)
        {
            var obj= await _master.GetAllEfAsync(a=>a.Id==id);
            return obj.FirstOrDefault();
                }

        public async Task<IEnumerable<Post>> GetTopPost(int number)
        {
            var obj =await _master.GetAllEfAsync();
            return obj.Take(number);
        }

        public async Task<int> PostCount()
        {
            var obj = await _master.GetAllEfAsync();
            return obj.Count();
        }
    }
}
