using Core.Dto.ViewModel.Dr;
using Core.Dto.ViewModel.Dr.QuestionFolder;
using Core.Service.Interface.Dr;
using Data;
using Data.MasterInterface;
using Data.Migrations;
using Domain.Dr;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Dr
{
    public class QuestionServices : IQuestion
    {
        private readonly IMaster<Question> _master;
        private readonly IMaster<DietQuestion> _masterDQ;
        private readonly IUserAnswer _userAnswer;

        public QuestionServices(IMaster<Question> master, IMaster<DietQuestion> masterDQ, IUserAnswer userAnswer)
        {
            _master = master;
            _masterDQ = masterDQ;
            _userAnswer = userAnswer;
        }

        public async Task<bool> BulkDeleteDietQuestion(IEnumerable<DietQuestion> dietquestions)
        {
            return await _masterDQ.BulkeDeleteAsync(dietquestions);
        }

        public async Task<bool> BulkInsertDietQuestion(IEnumerable<DietQuestion> dietquestions)
        {
            return await _masterDQ.BulkeInsertAsync(dietquestions.ToList());
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
          return await _master.GetAllEfAsync();
        }

        public async Task<IEnumerable<DietQuestion>> GetDietQuestionByDietIdAsync(int DietId,bool FirstForm)
        {
            return await _masterDQ.GetAllAsQueryable().Where(a => a.Diet.Id == DietId&&a.Question.FirstForm== FirstForm).Include(a => a.Diet).ToListAsync();
                }

        public async Task<IEnumerable<Question>> GetQuestionByDietIdAsync(int DietId,bool FirstForm)
        {
            var questions = await _master.GetAllAsQueryable()
                .Include(q => q.DietQuestions)
                .Where(q => q.DietQuestions.Any(dq => dq.DietId == DietId&& dq.Question.FirstForm== FirstForm))
                .ToListAsync();
            return questions;
        }

        public async Task<IEnumerable<QuestionWithUserAnswerVm>> GetQuestionWithAnswerOfUser(int UserId)
        {
          var question=await _master.GetAllAsQueryable().ToListAsync();
            var UserAnswer=await _userAnswer.GetAnswerOfUser(UserId);
            var model = question.Select(q => new QuestionWithUserAnswerVm
            {
                QuestionId = q.Id,
                QuestionText = q.Name,
                UserAnswer = UserAnswer.FirstOrDefault(a => a.QuestionId == q.Id)?.Answer
            }).ToList();
            return model;
        }
    }
}
