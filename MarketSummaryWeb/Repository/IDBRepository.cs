using MarketSummaryWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryWeb.Repository
{
    public interface IDBRepository
    {
        Task<IEnumerable<ProspectSearchCriteria>> GetExisitingProspectSearchCriteriaAsync(ProspectSearchCriteria prospectSearchCriteria);

        Task<ProspectSearchCriteria> GetProspectSearchCriteriaAsync(string id);

        Task<bool> CreateProspectSearchDataAsync(ProspectSearchCriteria ProspectSearchCriteria);

        Task<bool> UpdateProspectSearchDataAsync(ProspectSearchCriteria ProspectSearchCriteria);

        Task DeleteProspectSearchDataAsync(string id);
    }
}
