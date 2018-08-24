using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System.Configuration;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using MarketSummaryWeb.Models;

namespace MarketSummaryWeb.Repository
{
    public class CosmosRepository : IDBRepository 
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection"];
        private static DocumentClient client;

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(DatabaseId), new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> CreateProspectSearchDataAsync(ProspectSearchCriteria ProspectSearchCriteria)
        {

            Document doc = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), ProspectSearchCriteria);
            if (doc == null)
            {
                return false;
            }

            return true;            
        }

        public async Task<IEnumerable<ProspectSearchCriteria>> GetExisitingProspectSearchCriteriaAsync(ProspectSearchCriteria prospectSearchCriteria)
        {

            Expression<Func<ProspectSearchCriteria, bool>> predicate = null;
            IDocumentQuery<ProspectSearchCriteria> query = null;
            if (prospectSearchCriteria != null)
            {
                predicate = (p => p.ProspectName.ToLower() == prospectSearchCriteria.ProspectName.ToLower()
                            && p.SearchString.ToLower() == prospectSearchCriteria.SearchString.ToLower() && p.Id != prospectSearchCriteria.Id);
                query = client.CreateDocumentQuery<ProspectSearchCriteria>(
                        UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                        new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                        .Where(predicate)
                        .AsDocumentQuery();
            }
            else
            {
                query = client.CreateDocumentQuery<ProspectSearchCriteria>(
                       UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                       new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })                       
                       .AsDocumentQuery();
            }                  
            List<ProspectSearchCriteria> results = new List<ProspectSearchCriteria>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<ProspectSearchCriteria>());
            }

            return results;
        }

        public async Task<ProspectSearchCriteria> GetProspectSearchCriteriaAsync(string id)
        {
            try
            {
                Document document =
                    await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (ProspectSearchCriteria)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> UpdateProspectSearchDataAsync(ProspectSearchCriteria prospectSearchCriteria)
        {
            Document doc = await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, prospectSearchCriteria.Id.ToString()), prospectSearchCriteria);
            if (doc == null)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteProspectSearchDataAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id.ToString()));
        }
    }
}