using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryWeb.Repository
{
    public interface IDBRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetProspectsAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetProspectsAsync(int id);

        Task<bool> CreateSearchDataAsync(T ProspectSearchCriteria);

        Task<bool> UpdateSearchDataAsync(int id, T item);

        Task DeleteSearchDataAsync(int id);
    }
}
