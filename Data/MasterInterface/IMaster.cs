using Dapper;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.MasterInterface
{
    public interface IMaster<T> where T : class
    {

     
        Task<IEnumerable<T>> GetAllEfAsync();
        IQueryable<T> GetAllAsQueryable();
        IQueryable<T> GetAllAsQueryable(Expression<Func<T, bool>> Filter);
        Task<IEnumerable<T>> GetAllEfAsync(Expression<Func<T, bool>> Filter);
   
        Task<IEnumerable<T>> GetAllAsync(string spName, DynamicParameters parameters);
        Task<IEnumerable<T>> GetAllAsync(string spName);
        Task<IEnumerable<T>> GetPagingAsync(int Page, int pagesize);
        Task<int> GetNumberFromDatabaseAsync(string spName, object[] parameters);
        Task<string> GetStringFromDatabaseAsync(string spName, DynamicParameters parameters);
        Task<T> InsertAsync(T Obj);
        Task<bool> DeleteAsync(T Obj);
        Task<T> UpdateAsync(T Obj);
        Task<bool> BulkeInsertAsync(List<T> ListOfbulk);
        Task<bool> BulkeDeleteAsync(IEnumerable<T> ListOfbulk);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}