using Core.Dto.ViewModel.main;
using Domain;
using Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.MainPage
{
    public interface IComment
    {
        Task<bool> Insert(Comment comment);
        Task<bool> update(Comment comment);
        Task<IEnumerable<Comment>> GEtAllComments();
        Task<IEnumerable<Comment>> GEtAllUserComments();
        Task<Comment> GetCommentbyid(int CommentId);
        Task<Comment> GetRepolaybyid(int CommentId);
        Task<bool> ReplyToCommentAsync(ShowCommentVm comment, int AdminUserId);
        Task<IEnumerable<Comment>> GetCommentsByDietId(int DietId);
        Task<IEnumerable<Comment>> GetCommentsByEntityId(int entitytId,EntityType type);



        Task<IEnumerable<Comment>> GetAllCommentPaging(int pageid, int number);
        Task<IEnumerable<Comment>> GetTopComment(int number);
        Task<int> PostCount();
    }
}
