using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MarketSummaryWeb.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using AutoMapper;

namespace MarketSummaryWeb.Repository
{
    public class SQLRepository<T> : IDBRepository<T> where T : class
    {
        ProspectDBContext prospectDBContext = new ProspectDBContext();

        public async Task<IEnumerable<T>> GetProspectsAsync(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<ProspectDataSearchCriteria> prospectDataSearchCriteria = await prospectDBContext.ProspectDataSearchCriterias.ToListAsync<ProspectDataSearchCriteria>();
            IEnumerable<T> prospectSearchCriteriaList = Mapper.Map<IEnumerable<ProspectDataSearchCriteria>, IEnumerable<T>>(prospectDataSearchCriteria);
            return prospectSearchCriteriaList.AsQueryable().Where(predicate);

        }

        public async Task<T> GetProspectsAsync(int id)
        {
            ProspectDataSearchCriteria prospectDataSearchCriteria = await prospectDBContext.ProspectDataSearchCriterias.FindAsync(id);
            T prospectsearchCriteria = Mapper.Map<ProspectDataSearchCriteria, T>(prospectDataSearchCriteria);
            return prospectsearchCriteria;
        }

        public async Task<bool> CreateSearchDataAsync(T prospectSearchCriteria)
        {
            ProspectDataSearchCriteria prospectDataSearchCriteria = Mapper.Map<T, ProspectDataSearchCriteria>(prospectSearchCriteria);
            prospectDBContext.ProspectDataSearchCriterias.Add(prospectDataSearchCriteria);
            int result = await prospectDBContext.SaveChangesAsync();
            return result < 1 ? false : true;
        }

        public async Task<bool> UpdateSearchDataAsync(int id, T prospectSearchCriteria)
        {
            ProspectDataSearchCriteria prospectDataSearchCriteria = Mapper.Map<T, ProspectDataSearchCriteria>(prospectSearchCriteria);
            prospectDBContext.Entry(prospectDataSearchCriteria).State = EntityState.Modified;
            int result = await prospectDBContext.SaveChangesAsync();
            return result < 1 ? false : true;
        }

        public async Task DeleteSearchDataAsync(int id)
        {
            ProspectDataSearchCriteria ProspectDataSearchCriteria = await prospectDBContext.ProspectDataSearchCriterias.FindAsync(id);
            prospectDBContext.ProspectDataSearchCriterias.Remove(ProspectDataSearchCriteria);
            await prospectDBContext.SaveChangesAsync();
        }
    }
}