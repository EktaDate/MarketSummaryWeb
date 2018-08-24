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
    public class SQLRepository : IDBRepository
    {
        ProspectDBContext prospectDBContext = new ProspectDBContext();

        public async Task<IEnumerable<ProspectSearchCriteria>> GetExisitingProspectSearchCriteriaAsync(ProspectSearchCriteria prospectSearchCriteria)
        {
            Expression<Func<ProspectDataSearchCriteria, bool>> predicate = null;
            IEnumerable<ProspectDataSearchCriteria> prospectDataSearchCriteria = null;
            if (prospectSearchCriteria != null)
            {
                predicate = (p => p.ProspectName.ToLower() == prospectSearchCriteria.ProspectName.ToLower() 
                            && p.SearchString.ToLower() == prospectSearchCriteria.SearchString.ToLower() && p.Id != prospectSearchCriteria.Id);

                prospectDataSearchCriteria = await prospectDBContext.ProspectDataSearchCriterias.Where(predicate).ToListAsync<ProspectDataSearchCriteria>();
            }
            else
            {
                prospectDataSearchCriteria = await prospectDBContext.ProspectDataSearchCriterias.ToListAsync<ProspectDataSearchCriteria>();
            }                        
            IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = Mapper.Map<IEnumerable<ProspectDataSearchCriteria>, IEnumerable<ProspectSearchCriteria>>(prospectDataSearchCriteria);
            return prospectSearchCriteriaList.AsQueryable();

        }

        public async Task<ProspectSearchCriteria> GetProspectSearchCriteriaAsync(string id)
        {
            ProspectDataSearchCriteria prospectDataSearchCriteria = await prospectDBContext.ProspectDataSearchCriterias.FindAsync(int.Parse(id));
            ProspectSearchCriteria prospectsearchCriteria = Mapper.Map<ProspectDataSearchCriteria, ProspectSearchCriteria>(prospectDataSearchCriteria);
            return prospectsearchCriteria;
        }

        public async Task<bool> CreateProspectSearchDataAsync(ProspectSearchCriteria prospectSearchCriteria)
        {
            ProspectDataSearchCriteria prospectDataSearchCriteria = Mapper.Map<ProspectSearchCriteria, ProspectDataSearchCriteria>(prospectSearchCriteria);
            prospectDBContext.ProspectDataSearchCriterias.Add(prospectDataSearchCriteria);
            int result = await prospectDBContext.SaveChangesAsync();
            return result < 1 ? false : true;
        }

        public async Task<bool> UpdateProspectSearchDataAsync(ProspectSearchCriteria prospectSearchCriteria)
        {
            ProspectDataSearchCriteria prospectDataSearchCriteria = Mapper.Map<ProspectSearchCriteria, ProspectDataSearchCriteria>(prospectSearchCriteria);
            prospectDBContext.Entry(prospectDataSearchCriteria).State = EntityState.Modified;
            int result = await prospectDBContext.SaveChangesAsync();
            return result < 1 ? false : true;
        }

        public async Task DeleteProspectSearchDataAsync(string id)
        {
            ProspectDataSearchCriteria ProspectDataSearchCriteria = await prospectDBContext.ProspectDataSearchCriterias.FindAsync(int.Parse(id));
            prospectDBContext.ProspectDataSearchCriterias.Remove(ProspectDataSearchCriteria);
            await prospectDBContext.SaveChangesAsync();
        }
    }
}