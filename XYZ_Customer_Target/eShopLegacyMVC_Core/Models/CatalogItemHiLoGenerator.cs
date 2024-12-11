using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eShopLegacyMVC_Core.Models
{
    public class CatalogItemHiLoGenerator
    {
        private const int HiLoIncrement = 10;
        private int sequenceId = -1;
        private int remainingLoIds = 0;
        private readonly object sequenceLock = new object();
        private readonly ILogger<CatalogItemHiLoGenerator> _logger;

        public CatalogItemHiLoGenerator(ILogger<CatalogItemHiLoGenerator> logger)
        {
            _logger = logger;
        }

        public async Task<int> GetNextSequenceValueAsync(CatalogDBContext db)
        {
            // Lock the critical section to prevent race conditions on sequenceId and remainingLoIds
            lock (sequenceLock)
            {
                // If no remaining Lo IDs, fetch the next batch
                if (remainingLoIds == 0)
                {
                    _logger.LogInformation("Fetching next HiLo sequence value from database asynchronously.");
                    
                    // Perform the async DB operation outside of the lock block
                    return Task.Run(async () => await GetNextSequenceValueFromDbAsync(db)).Result; // Get the value synchronously after the async operation
                }
                else
                {
                    // Decrement remainingLoIds and use cached sequenceId
                    remainingLoIds--;
                    _logger.LogInformation("Using cached sequence ID: {SequenceId}", sequenceId + 1);
                    return ++sequenceId;
                }
            }
        }

        private async Task<int> GetNextSequenceValueFromDbAsync(CatalogDBContext db)
        {
            try
            {
                // Perform the async DB operation outside the lock block
                var rawQuery = await db.Database.ExecuteSqlRawAsync("SELECT NEXT VALUE FOR catalog_hilo;");
                sequenceId = Convert.ToInt32(rawQuery);
                remainingLoIds = HiLoIncrement - 1;

                _logger.LogInformation("New sequence ID obtained: {SequenceId}", sequenceId);
                return sequenceId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating next sequence value asynchronously.");
                throw;
            }
        }
    }
}
