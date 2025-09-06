using Core.Dto.ViewModel.Dr;
using Core.Dto.ViewModel.Dr.DietVM;
using Core.Dto.ViewModel.main;
using Core.Enums;
using Core.Extention;
using Core.Service.Interface.Dr;
using Dapper;
using Data;
using Data.MasterInterface;
using Domain;
using Domain.Dr;
using Domain.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Dr
{
  
    public class UserDietServices : IUserDiet
    {
        private readonly IMaster<UserDiet> _master;
        private readonly IFileList _fileList;
        private readonly IUserAnswer _userAnswer;
        private readonly IDiet _diet;
        private readonly IMaster<ShowUserDietPanelVm> _Vm;

        public UserDietServices(IMaster<UserDiet> master, IFileList fileList, IUserAnswer userAnswer, HttpClient httpClient, IDiet diet, IMaster<ShowUserDietPanelVm> vm)
        {
            _master = master;
            _fileList = fileList;
            _userAnswer = userAnswer;
            _diet = diet;
            _Vm = vm;
        }



        public async Task<bool> DeleteUserDiet(UserDiet userDiet)
        {
            var obj =await _master.DeleteAsync(userDiet);
            return obj;
        }

        public async Task<Paging<ShowUserDietPanelVm>> GetAllDietsByFilter(
       int? userId, string paymentStatus, string fullName, string mobile,
       int pageNumber, int pageSize)
        {

            
            var parameters = new DynamicParameters();
            parameters.Add("@userId", userId);
            parameters.Add("@paymentStatus", string.IsNullOrEmpty(paymentStatus) ? null : paymentStatus);
            parameters.Add("@fullName", string.IsNullOrEmpty(fullName) ? null : fullName);
            parameters.Add("@mobile", string.IsNullOrEmpty(mobile) ? null : mobile);
            parameters.Add("@PageNumber", pageNumber);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var items = await _Vm.GetAllAsync("GetAllDietsByFilter", parameters);

            return new Paging<ShowUserDietPanelVm>
            {
                bjects= items.ToList(),
                TotalCount = parameters.Get<int>("@TotalCount"),
                pageNumber = pageNumber,
                pageSize = pageSize,
                fullName = fullName,
                mobile = mobile,
                paymentStatus = paymentStatus,
                userId = userId 
            
            };
        }

        public async Task<IEnumerable<ShowUserDietPanelVm>> GetAllDietsByUserId(int userId)
{

            DynamicParameters p=new DynamicParameters();
            p.Add("userId", userId == 0 ? null : userId, DbType.Int32);
            return await _Vm.GetAllAsync("GetAllDietsByUserId", p);

        }


        public async Task<IEnumerable<UserDiet>> GetAllDietsByUserIdAndDietId(int UserId,int DietId)
        {
            return await _master.GetAllAsQueryable().Include(a=>a.diet).Include(a=>a.User).Where(A => A.UserId == UserId&&A.DietId==DietId).ToListAsync();
        }

        public Task<IEnumerable<UserDiet>> GetAllDietsByUserIdAnother(int UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDiet>> GetAllParentAndChild(int userDietId)
        {
          
            var p =await GetUserDietById(userDietId);
       
            var all = p?.ParentId ?? 0;
        var  obj = await _master.GetAllEfAsync(a =>
       a.Id == userDietId ||
       a.ParentId == userDietId ||
       (all != 0 && (a.Id == all || a.ParentId == all)));
            return obj;
        }

        public async Task<IEnumerable<UserDiet>> GetAllUserDiets()
        {
            return await _master.GetAllAsQueryable().Include(a => a.diet).Include(a => a.User).OrderBy(a=>a.DeleteTime).ToListAsync();
        }

        public async Task<UserDiet> GetUserDietByAuthority(string Authority)
        {
            var obj =await _master.GetAllEfAsync(a => a.PaymentAuthority == Authority);
            return obj.FirstOrDefault();
        }

        public async Task<UserDiet> GetUserDietById(int UserDietId)
        {
            var obj = await _master.GetAllAsQueryable().Include(a=>a.User).Include(a=>a.diet).FirstOrDefaultAsync(a => a.Id == UserDietId);
            return obj;
        }

        public async Task<bool> InsertAnswerAsync(ShowQuestionToUserVM vm,int UserId)
        {
            using var transaction = await _master.BeginTransactionAsync();
            var uploadedFiles = new List<string>();
            var diet =await _diet.GetDietById(vm.DietId);

            try
            {
                var userDiet = await InsertUserDiet(new UserDiet
                {
                    DietId = vm.DietId,
                    UserId = UserId,
                    Status = UserDietstatus.FillForm,
                    Amount=diet.Price,
                    ParentId=vm.parentId.Value,
                    CreateAt=DateTime.UtcNow
                });
                foreach (var q in vm.Questions)
                {
                    if (q.Attachments != null)
                    {
                        foreach (var file in q.Attachments)
                        {
                            var uploadResult = FileTools.UploadFile(file, FileTools.GetFileName(file), "Attachment");
                            if (!uploadResult.Success)
                                throw new Exception($"فایل {uploadResult.FilePath} آپلود نشد.");
                            uploadedFiles.Add(uploadResult.FilePath); // برای پاک کردن در صورت خطا
                            await _fileList.InsertFile(new FileList
                            {
                                File = uploadResult.FilePath,
                                UserDietId = userDiet.Id,
                                UserFile = true
                            });
                        }
                    }
                }
                var answers = vm.Questions
          .Where(q => q.IsRequired)
          .Select(q => new UserAnswer
          {
              DietId = vm.DietId,
              QuestionId = q.QuestionId,
              UserId = UserId,
              Answer = q.Answer,
              UserDietId = userDiet.Id
          }).ToList();
                var answerRes = await _userAnswer.BulkInsertUserAnswer(answers);
                if (!answerRes) throw new Exception("ثبت پاسخ‌ها انجام نشد.");
                await transaction.CommitAsync();
             
                    return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                foreach (var path in uploadedFiles)
                {
                    try
                    {

                        FileTools.DeleteFile(path);
                    }
                    catch { /* لاگ خطا */ }
                }
                return false;
            }

        }

        public async Task<UserDiet> InsertUserDiet(UserDiet userDiet)
        {
            var obj=await _master.InsertAsync(userDiet);
            return obj;
        }

        public async Task<bool> UpdateToFinaltPay(UserDiet userDiet)
        {
            var obj =await _master.UpdateAsync(userDiet);
            return obj != null;
        }

        public async Task<bool> UpdateToFirstPay(int UserdietId, string Authority)
        {
            var diet=await GetUserDietById(UserdietId);
            diet.PaymentAuthority = Authority;
           return await _master.UpdateAsync(diet)!=null;

        }

        public async Task<bool> UpdateToSend(int UserDietId)
        {
            var obj = await GetUserDietById(UserDietId);
            obj.Status = Domain.UserDietstatus.send;
            var res = await _master.UpdateAsync(obj);
            return res != null;
        }

        public async Task<bool> UserHasDiet(int UserId, int DietId)
        {
            var res = await _master.GetAllEfAsync();
            return res.Any(a => a.UserId == UserId && a.DietId == DietId)?true:false;
        }
    }
}
