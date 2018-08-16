using MarketSummaryWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryWeb.Repository
{
    interface IDataAccess
    {
        Task<IEnumerable<ProspectSearchCriteria>> GetProspectSearchCriteriaAsync();

        Task<ProspectSearchCriteria> GetProspectSearchCriteriaAsync(int id);

        Task<ProspectSearchCriteria> GetProspectSearchCriteriaAsync(Expression<Func<ProspectSearchCriteria, bool>> predicate);

        Task<bool> InsertProspectSearchData(ProspectSearchCriteria prospectSearchCriteria);

        Task<bool> UpdateProspectSearchData(ProspectSearchCriteria prospectSearchCriteria);

        Task<bool> DeleteProspectSearchData(ProspectSearchCriteria prospectSearchCriteria);

    }
}
