using Azure;
using Core.Dto.ViewModel.main;
using Core.Service.Interface.Dr;
using Core.Service.Interface.MainPage;
using Data.MasterInterface;
using Domain;
using Domain.Main;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Service.Services.MainPage
{
    public class CommentServices : IComment
    {
        private readonly IMaster<Comment> _master;

        public CommentServices(IMaster<Comment> master)
        {
            _master = master;
        }

        public async Task<IEnumerable<Comment>> GEtAllComments()
        {


            //var comments = await _master.GetAllAsQueryable().ToListAsync();
            //var dietid = comments.Where(a => a.EntityType == Domain.EntityType.Diet & a.EntityId != null)
            //    .Select(a => a.EntityId.Value).Distinct();
            //var diets = await _diet.GetDietByIds(dietid);
            //    var result = comments.Select(c => new Comment
            //    {
            //        Id = c.Id,
            //        Text = c.Text,
            //        EntityId = c.EntityType == Domain.EntityType.Diet
            //? diets.FirstOrDefault(d => d.Id == c.EntityId)?.Name
            //: products.FirstOrDefault(p => p.Id == c.EntityId)?.Name
            //    }).ToList();
            return await _master.GetAllAsQueryable().Include(a => a.myUser).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllCommentPaging(int pageid, int number, int UserId)
        {

            var obj = await _master
     .GetAllAsQueryable()
     .Include(a => a.myUser)
     .Where(a => a.UserId!=UserId) // فقط سؤال‌ها
     .OrderByDescending(a => a.Id)
     .Select(a => new Comment
     {
        Id= a.Id,
        EntityId= a.EntityId,
        Text= a.Text, // فرضی
         Name = string.IsNullOrEmpty(a.Name) ? a.myUser.FullName : a.Name,
    CreateDate=a.CreateDate,
    IsApproved=a.IsApproved,
    EntityType=a.EntityType
     })
     .Skip((pageid - 1) * number)
     .Take(number)
     .ToListAsync();
            return obj;
            
        }

        public async Task<IEnumerable<Comment>> GEtAllUserComments()
        {
            var obj = await _master.GetAllEfAsync(a => a.ParentId == null);
           
            return obj.OrderByDescending(a=>a.CreateDate);
        }

        public async Task<Comment> GetCommentbyid(int CommentId)
        {
            var obj = await _master.GetAllAsQueryable(a => a.Id == CommentId).Include(a=>a.myUser).ToListAsync();
            return obj.FirstOrDefault();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByDietId(int DietId)
        {
            var obj = await _master.GetAllAsQueryable().Include(a => a.myUser).Where(a => a.EntityId == DietId & a.IsApproved == true).ToListAsync();
            return obj;
        }

        public async Task<Comment> GetRepolaybyid(int CommentId)
        {
            var obj = await _master.GetAllEfAsync(a => a.ParentId == CommentId);
            return obj.FirstOrDefault();
        }

        public async Task<IEnumerable<Comment>> GetTopComment(int number)
        {
            var obj = await _master.GetAllEfAsync();
            return obj.Take(number);
        }

        public async Task<bool> Insert(Comment comment)
        {
            var obj = await _master.InsertAsync(comment);
            return obj != null;
        }

        public async Task<int> PostCount()
        {
            var obj = await _master.GetAllEfAsync(a=>a.ParentId==null);
            return obj.Count();
        }

        public async Task<bool> ReplyToCommentAsync(ShowCommentVm vm, int AdminUserId)
        {
            bool fiResult = true;
            var comment =await GetCommentbyid(vm.Id);
            if (comment == null)
                return false;
            if(comment.IsApproved!=vm.IsApproved)
            {
                comment.IsApproved = vm.IsApproved;
                var UpResult = await update(comment);
                if (!UpResult)
                    return false;
            }
           
            if(!string.IsNullOrEmpty(vm.AdminComment))
            {
                var reply = await GetRepolaybyid(vm.Id);
                if (reply == null)
                {
                    // first time
                    reply = new Comment
                    {
                        ParentId = vm.Id,
                        Text = vm.AdminComment,
                        CreateDate = DateTime.Now,
                        IsApproved = true,
                        UserId = AdminUserId,
                        EntityId=vm.DietId,
                        EntityType=vm.EntityType
                        
                    };

                    fiResult = await Insert(reply);
                }
                else
                {
                    reply.Text = vm.AdminComment;
                    fiResult = await update(reply);
                }
                return fiResult;
            }
            return fiResult;
        }

        public async Task<bool> update(Comment comment)
        {
            var obj = await _master.UpdateAsync(comment);
            return obj != null;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByEntityId(int entitytId,Domain.EntityType type)
        {

            var obj = await _master.GetAllAsQueryable().Include(a => a.myUser).Where(c => c.EntityId == entitytId
                     && c.EntityType == type
                     && c.IsApproved)
            .OrderBy(c => c.CreateDate).Select(a => new Comment
            {
                Id = a.Id,
                EntityId = a.EntityId,
                Text = a.Text, // فرضی
                Name = string.IsNullOrEmpty(a.Name) ? a.myUser.FullName : a.Name,
                CreateDate = a.CreateDate,
                IsApproved = a.IsApproved,
                EntityType = a.EntityType,
                ParentId=a.ParentId
            }).ToListAsync();



            return obj;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserId(int UserId)
        {
            return await _master.GetAllAsQueryable().Include(a => a.myUser)
     .Where(a => a.UserId == UserId) // فقط سؤال‌ها
     .OrderByDescending(a => a.Id)
     .Select(a => new Comment
     {
         Id = a.Id,
         EntityId = a.EntityId,
         Text = a.Text, // فرضی
         Name =  a.myUser.FullName ,
         CreateDate = a.CreateDate,
         IsApproved = a.IsApproved,
         EntityType = a.EntityType
     }).ToListAsync();
  
        }
    }
}
