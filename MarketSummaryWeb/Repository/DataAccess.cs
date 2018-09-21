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
            IDBRepository dbObejct = FactoryClass.CreateDBRepositoryObject();
            IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = await dbObejct.GetExisitingProspectSearchCriteriaAsync(null);
            return prospectSearchCriteriaList.OrderBy(p => p.ProspectName).ThenBy(k => k.SearchString);
        }

        public async Task<IEnumerable<ProspectMarketSummary>> GetProspectSummaryAsync(string prospectName)
        {
            IDBRepository dbObejct = FactoryClass.CreateDBRepositoryObject();
            IEnumerable<ProspectMarketSummary> prospectSearchCriteriaList = await dbObejct.GetProspectSummaryDataAsync(prospectName);
            return prospectSearchCriteriaList.OrderBy(p => p.ProspectName);
        }

        public async Task<ProspectSearchCriteria> GetProspectSearchCriteriaAsync(int id,string rowKey)
        {
            IDBRepository dbObejct = FactoryClass.CreateDBRepositoryObject();
            string key = string.IsNullOrEmpty(rowKey) ? id.ToString() : rowKey; 
            ProspectSearchCriteria prospectSearchCriteria = await dbObejct.GetProspectSearchCriteriaAsync(key);
            return prospectSearchCriteria;
        }

        public async Task<ProspectSearchCriteria> GetProspectSearchCriteriaAsync(ProspectSearchCriteria prospectSearchCriteria)
        {
            IDBRepository dbObejct = FactoryClass.CreateDBRepositoryObject();            
            IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = await dbObejct.GetExisitingProspectSearchCriteriaAsync(prospectSearchCriteria);
            return prospectSearchCriteriaList.FirstOrDefault();
        }

        public async Task<bool> InsertProspectSearchData(ProspectSearchCriteria prospectSearchCriteria)
        {
            try
            {
                IDBRepository dbObejct = FactoryClass.CreateDBRepositoryObject();
                return await dbObejct.CreateProspectSearchDataAsync(prospectSearchCriteria);
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
                IDBRepository dbObejct = FactoryClass.CreateDBRepositoryObject();
                return await dbObejct.UpdateProspectSearchDataAsync( prospectSearchCriteria);
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
                IDBRepository dbObejct = FactoryClass.CreateDBRepositoryObject();
                string key = string.IsNullOrEmpty(prospectSearchCriteria.RowKey) ? prospectSearchCriteria.Id.ToString() : prospectSearchCriteria.RowKey;
                await dbObejct.DeleteProspectSearchDataAsync(key);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}