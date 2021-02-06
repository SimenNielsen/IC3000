using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IC3000.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;

namespace IC3000.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddClaimAsync(Claim claim)
        {
            await this._container.CreateItemAsync<Claim>(claim, new PartitionKey(claim.Id));
        }

        public async Task DeleteClaimAsync(string id)
        {
            await this._container.DeleteItemAsync<Claim>(id, new PartitionKey(id));
        }

        public async Task<Claim> GetClaimAsync(string id)
        {
            try
            {
                ItemResponse<Claim> response = await this._container.ReadItemAsync<Claim>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Claim>(new QueryDefinition(queryString));
            List<Claim> results = new List<Claim>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateClaimAsync(string id, Claim claim)
        {
            await this._container.UpsertItemAsync<Claim>(claim, new PartitionKey(id));
        }
    }
}
