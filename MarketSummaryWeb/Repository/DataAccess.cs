using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MarketSummaryWeb.Models;
using System.Linq.Expressions;
namespace MarketSummaryWeb.Repository
{
    public sealed class DataAccess : IDataAccess
    {
        private static DataAccess instance;
        private DataAccess()
        {

        }        

        public static DataAccess GetInstance()
        {
            if (instance == null)
            {
                instance = new DataAccess();
            }

            return instance;
        }

        public async Task<IEnumerable<ProspectSearchCriteria>> GetProspectSearchCriteriaAsync()
        {
            IDBRepository<ProspectSearchCriteria> dbObejct = FactoryClass<ProspectSearchCriteria>.CreateDBRepositoryObject();
            IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = await dbObejct.GetProspectsAsync(d => d.Id != null);
            return prospectSearchCriteriaList.OrderBy(p => p.ProspectName).ThenBy(k => k.SearchString);
        }

        public async Task<ProspectSearchCriteria> GetProspectSearchCriteriaAsync(int id)
        {
            IDBRepository<ProspectSearchCriteria> dbObejct = FactoryClass<ProspectSearchCriteria>.CreateDBRepositoryObject();
            ProspectSearchCriteria prospectSearchCriteria = await dbObejct.GetProspectsAsync(id);
            return prospectSearchCriteria;
        }

        public async Task<ProspectSearchCriteria> GetProspectSearchCriteriaAsync(Expression<Func<ProspectSearchCriteria, bool>> predicate)
        {
            IDBRepository<ProspectSearchCriteria> dbObejct = FactoryClass<ProspectSearchCriteria>.CreateDBRepositoryObject();            
            IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = await dbObejct.GetProspectsAsync(predicate);
            return prospectSearchCriteriaList.FirstOrDefault();
        }

        public async Task<bool> InsertProspectSearchData(ProspectSearchCriteria prospectSearchCriteria)
        {
            try
            {
                IDBRepository<ProspectSearchCriteria> dbObejct = FactoryClass<ProspectSearchCriteria>.CreateDBRepositoryObject();
                return await dbObejct.CreateSearchDataAsync(prospectSearchCriteria);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProspectSearchData(ProspectSearchCriteria prospectSearchCriteria)
        {
            try
            {
                IDBRepository<ProspectSearchCriteria> dbObejct = FactoryClass<ProspectSearchCriteria>.CreateDBRepositoryObject();
                return await dbObejct.UpdateSearchDataAsync(prospectSearchCriteria.Id, prospectSearchCriteria);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteProspectSearchData(ProspectSearchCriteria prospectSearchCriteria)
        {
            try
            {
                IDBRepository<ProspectSearchCriteria> dbObejct = FactoryClass<ProspectSearchCriteria>.CreateDBRepositoryObject();
                await dbObejct.DeleteSearchDataAsync(prospectSearchCriteria.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}