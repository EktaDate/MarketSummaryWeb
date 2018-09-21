using MarketSummaryWeb.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace MarketSummaryWeb.Repository
{
    public class TableStorageRepository : IDBRepository
    {
        const string TABLENAME = "ProspectSearchCriteria";

        public CloudTable GetTableManager(string _CloudTableName)
        {
            CloudTable table;
            try
            {
                string ConnectionString = ConfigurationManager.AppSettings["tableStorageConnection"];
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                table = tableClient.GetTableReference(_CloudTableName);
                table.CreateIfNotExists();
                return table;
            }
            catch (StorageException StorageExceptionObj)
            {
                throw StorageExceptionObj;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        private async Task<IEnumerable<ProspectSearchCriteria>> retrieveProspectSearchCriteriaDetails(string query)
        {
            CloudTable table = GetTableManager(TABLENAME);
            TableQuery<ProspectSearchCriteria> tableQuery = new TableQuery<ProspectSearchCriteria>();
            if (!string.IsNullOrWhiteSpace(query))
            {
                tableQuery = new TableQuery<ProspectSearchCriteria>().Where(query);
            }
            IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = table.ExecuteQuery(tableQuery);
            return prospectSearchCriteriaList;
        }

        public async Task<IEnumerable<ProspectSearchCriteria>> GetExisitingProspectSearchCriteriaAsync(ProspectSearchCriteria prospectSearchCriteria)
        {
            try
            {
                string query = string.Empty;
                if (prospectSearchCriteria != null )
                {
                    query = "RowKey ne '" + prospectSearchCriteria.RowKey + "' and ProspectName eq '" + prospectSearchCriteria.ProspectName +
                            "' and SearchString eq '" + prospectSearchCriteria.SearchString + "'";
                }                
                IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = await retrieveProspectSearchCriteriaDetails(query);
                return prospectSearchCriteriaList;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProspectSearchCriteria> GetProspectSearchCriteriaAsync(string rowKey)
        {
            try
            {
                string query = "RowKey eq '" + rowKey.ToString() + "'";
                IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = await retrieveProspectSearchCriteriaDetails(query);
                if (prospectSearchCriteriaList != null && prospectSearchCriteriaList.Count() > 0)
                {
                    return prospectSearchCriteriaList.FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public async Task<bool> CreateProspectSearchDataAsync(ProspectSearchCriteria prospectSearchCriteria)
        {
            try
            {
                CloudTable table = GetTableManager(TABLENAME);
                prospectSearchCriteria.PartitionKey = "Prospects";
                prospectSearchCriteria.RowKey = Guid.NewGuid().ToString();
                TableOperation insertOperation = TableOperation.Insert(prospectSearchCriteria);
                await table.ExecuteAsync(insertOperation);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateProspectSearchDataAsync(ProspectSearchCriteria prospectSearchCriteria)
        {
            try
            {
                CloudTable table = GetTableManager(TABLENAME);
                prospectSearchCriteria.ETag = "*";                
                TableOperation updateOperation = TableOperation.Replace(prospectSearchCriteria);
                await table.ExecuteAsync(updateOperation);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task DeleteProspectSearchDataAsync(string rowKey)
        {
            try
            {
                CloudTable table = GetTableManager(TABLENAME);
                string query = "RowKey eq '" + rowKey + "'";
                IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = await retrieveProspectSearchCriteriaDetails(query);
                if (prospectSearchCriteriaList != null && prospectSearchCriteriaList.Count() > 0)
                {
                    ProspectSearchCriteria prospectSearchCriteria = prospectSearchCriteriaList.FirstOrDefault();
                    TableOperation deleteOperation = TableOperation.Delete(prospectSearchCriteria);
                    await table.ExecuteAsync(deleteOperation);
                }
            }
            catch (StorageException StorageExceptionObj)
            {
                throw StorageExceptionObj;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }

        }

        public async Task<IEnumerable<ProspectMarketSummary>> GetProspectSummaryDataAsync(string prospectName)
        {
            string query = string.Empty;
            if (string.IsNullOrWhiteSpace(query))
            {
                query = "ProspectName eq '"+prospectName+"'";
            }
            CloudTable table = GetTableManager(TABLENAME);
            TableQuery<ProspectMarketSummary> tableQuery = new TableQuery<ProspectMarketSummary>();
            if (!string.IsNullOrWhiteSpace(query))
            {
                tableQuery = new TableQuery<ProspectMarketSummary>().Where(query);
            }
            IEnumerable<ProspectMarketSummary> prospectSearchCriteriaList = table.ExecuteQuery(tableQuery);
            return prospectSearchCriteriaList;
        }
    }
}