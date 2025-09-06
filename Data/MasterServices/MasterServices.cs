using Dapper;
using Data.MasterInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using EventId = Domain.EventIdList;


namespace Data.MasterServices
{
    public class MasterServices<T> : IMaster<T> where T : class
    {

        protected MyContext _ctx;
        protected IDbConnection cnnsql;
        protected readonly string cnn;
        private readonly IHttpContextAccessor _accessor;

        private readonly ILogger _logger;

        public MasterServices(MyContext ctx, ILoggerFactory factory, IHttpContextAccessor accessor)
        {
            _ctx = ctx;
            cnn = ctx.Database.GetConnectionString();
            cnnsql = new SqlConnection(cnn);
            _logger = factory.CreateLogger("DrMoradi");
            _accessor = accessor;
            //cnnsql = new MySql.Data.MySqlClient.MySqlConnection(cnn);mysql
        }
   

     

        public async Task<IEnumerable<T>> GetAllAsync(string spName, DynamicParameters parameters)
        {
            IEnumerable<T> obj = null;

            try
            {

                using (var connection = new SqlConnection(cnn))
                {
                    obj =await cnnsql.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(EventId.Error, ex, "(Data Error) User Run GetAll With sp and Get Error  ObjectName= {ObjectName}  and parameter and UserId = {UserId}", typeof(T).Name, GetUserId());
                return Enumerable.Empty<T>(); ;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string spName)
        {
            IEnumerable<T> obj = null;

            try
            {

                using (IDbConnection connection = new SqlConnection(cnn))
                {
                    obj =await cnnsql.QueryAsync<T>(spName, null, commandType: CommandType.StoredProcedure);
                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(EventId.Error, ex, "(Data Error) User Run GetAllVm ObjectName= {ObjectName} and UserId = {UserId}", typeof(T).Name, GetUserId());
                return obj;
            }
        }

        public async Task<IEnumerable<T>> GetPagingAsync(int page, int pageSize)
        {

            int skipRows = (page - 1) * pageSize;
           return await _ctx.Set<T>()
    .Skip(skipRows)
    .Take(pageSize)
    .ToListAsync();
            
        }

        public Task<int> GetNumberFromDatabaseAsync(string spName, object[] parameters)
        {
            if (parameters == null)
            {
                return _ctx.Database.ExecuteSqlRawAsync(spName);
            }
            else
            {
                return _ctx.Database.ExecuteSqlRawAsync(spName, parameters);
            }
        }

        public async Task<string> GetStringFromDatabaseAsync(string spName, DynamicParameters parameters)
        {
            return await cnnsql.ExecuteScalarAsync<string>(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> InsertAsync(T Obj)
        {
            try
            {
             await   _ctx.AddAsync(Obj);
              await  _ctx.SaveChangesAsync();
                return Obj;
            }
            catch (Exception e)
            {
                _logger.LogError(eventId: (int)EventId.InsertId, e, "(Data Error) User Run Insert and Get Error  ObjectName= {ObjectName} and userId = {userId} and return null", typeof(T).Name, GetUserId());
                return null;
            }

        }

        public async Task<bool> DeleteAsync(T Obj)
        {
            try
            {
                _ctx.Remove(Obj);
               await _ctx.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {
                _logger.LogError(eventId: (int)EventId.DeleteId, e, "(Data Error) User Run Delete and Get Error  ObjectName= {ObjectName} and userId = {userId} and return False", typeof(T).Name, GetUserId());

                return false;
            }
        }

        public async Task<T> UpdateAsync(T Obj)
        {
            try
            {
                // مقدار Id رو با Reflection می‌گیریم
                var idProperty = typeof(T).GetProperty("Id");
                if (idProperty == null)
                {
                    _ctx.Attach(Obj);
                    _ctx.Entry(Obj).State = EntityState.Modified;
                    await _ctx.SaveChangesAsync();

                    return Obj;
                    //throw new InvalidOperationException("Entity must have an Id property");

                }

                var idValue = idProperty.GetValue(Obj);
                if (idValue == null)
                    throw new InvalidOperationException("Id value cannot be null.");
                // بررسی اینکه آیا entity مشابه در حال track شدن هست
                var tracked = _ctx.ChangeTracker.Entries<T>()
                    .FirstOrDefault(e =>
                        e.State != EntityState.Detached &&
                        e.Property("Id").CurrentValue.Equals(idValue));

                // اگر entity مشابهی track شده، detachش کن
                if (tracked != null)
                {
                    tracked.State = EntityState.Detached;
                }

                // حالا entity رو attach و update می‌کنیم
                _ctx.Attach(Obj);
                _ctx.Entry(Obj).State = EntityState.Modified;
               await _ctx.SaveChangesAsync();

                return Obj;

            }
            catch (Exception e)
            {
                _logger.LogError(eventId: (int)EventId.UpdateId, e, "(Data Error) User Run Update and Get Error  ObjectName= {ObjectName} and userId = {userId} and return null", typeof(T).Name, GetUserId());

                return null;
            }

        }

        public async Task<bool> BulkeInsertAsync(List<T> ListOfbulk)
        {
            try
            {

               await _ctx.Set<T>().AddRangeAsync(ListOfbulk);
               await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(eventId: (int)EventId.BulkInsertId, e, "(Data Error) User Run BulkInsert and Get Error  ObjectName= {ObjectName} and userId = {userId} and return false", typeof(T).Name, GetUserId());
                return false;
            }
        }

        public async Task<bool> BulkeDeleteAsync(IEnumerable<T> ListOfbulk)
        {
            try
            {

                _ctx.Set<T>().RemoveRange(ListOfbulk);
               await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(eventId: (int)EventId.BulkeDelete, e, "(Data Error) User Run BulkDelete and Get Error  ObjectName= {ObjectName} and userId = {userId} and return false", typeof(T).Name, GetUserId());
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllEfAsync()
        {
            try
            {
                return await _ctx.Set<T>().ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(EventId.Error, ex, "(Data Error) User Run GetAllEf ObjectName= {ObjectName}  UserId = {UserId}", typeof(T).Name, GetUserId());

                return null;
            }
        }
        public async Task<IEnumerable<T>> GetAllEfAsync(Expression<Func<T, bool>> Filter)
        {
            try
            {


                var obj = _ctx.Set<T>().AsQueryable();
                if (Filter != null)
                {
                    obj = obj.Where(Filter);
                }


                return await obj.ToListAsync();
            }
            
            catch (Exception ex)
            {
                var ExType=ex.GetType().Name;
                var message = ex.Message;
                _logger.LogError(EventId.Error, ex, "(Data Error) User Run GetAllEfAsync ObjectName= {ObjectName} With expression type of {type} and message ={message} and UserId = {UserId}", typeof(T).Name, ExType, message,GetUserId());
                 throw;
            }


        }
        public IQueryable<T> GetAllAsQueryable(Expression<Func<T, bool>> Filter)
        {
             try
            {


                var obj = _ctx.Set<T>().AsQueryable();
                if (Filter != null)
                {
                    obj = obj.Where(Filter);
                }


                return  obj;
            }
            
            catch (Exception ex)
            {
                var ExType=ex.GetType().Name;
                var message = ex.Message;
                _logger.LogError(EventId.Error, ex, "(Data Error) User Run GetAllAsQueryable ObjectName= {ObjectName} With expression type of {type} and message ={message} and UserId = {UserId}", typeof(T).Name, ExType, message,GetUserId());
                 throw;
            }
        }
        private int GetUserId()
        {
            try
            {
                var idStr = _accessor.HttpContext?.User?.Claims
                    ?.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

                return int.TryParse(idStr, out var userId) ? userId : 0;
            }
            catch
            {
                return 0;
            }
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            try
            {
                return _ctx.Set<T>().AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.LogError(EventId.Error, ex, "(Data Error) User Run GetAllAsQueryable ObjectName= {ObjectName}  UserId = {UserId}", typeof(T).Name, GetUserId());
                return Enumerable.Empty<T>().AsQueryable();
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _ctx.Database.BeginTransactionAsync();
        }

      
    }
}
