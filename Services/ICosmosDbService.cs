using IC3000.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IC3000.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Claim>> GetClaimsAsync(string query);
        Task<Claim> GetClaimAsync(string id);
        Task AddClaimAsync(Claim claim);
        Task UpdateClaimAsync(string id, Claim claim);
        Task DeleteClaimAsync(string id);
    }
}
