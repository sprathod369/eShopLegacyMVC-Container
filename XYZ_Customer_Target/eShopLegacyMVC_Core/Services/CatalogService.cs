using eShopLegacyMVC_Core.Models; 
using eShopLegacyMVC_Core.ViewModel; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 
namespace eShopLegacyMVC_Core.Services 
{ 
    public class CatalogService : ICatalogService 
    { 
        private readonly CatalogDBContext _db; 
        private readonly CatalogItemHiLoGenerator _indexGenerator; 
        private readonly ILogger<CatalogService> _logger; 
        public CatalogService(CatalogDBContext db, CatalogItemHiLoGenerator indexGenerator, ILogger<CatalogService> logger) 
        { 
            _db = db ?? throw new ArgumentNullException(nameof(db)); 
            _indexGenerator = indexGenerator ?? throw new ArgumentNullException(nameof(indexGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        } 
        public async Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItemsPaginatedAsync(int pageSize, int pageIndex) 
        { 
            try 
            { 
                _logger.LogInformation("Fetching paginated catalog items asynchronously: pageSize={PageSize}, pageIndex={PageIndex}", pageSize, pageIndex); 
                var totalItems = await _db.CatalogItems.LongCountAsync(); 
                var itemsOnPage = await _db.CatalogItems 
                    .Include(c => c.CatalogBrand) 
                    .Include(c => c.CatalogType) 
                    .OrderBy(c => c.Id) 
                    .Skip(pageSize * pageIndex) 
                    .Take(pageSize) 
                    .ToListAsync(); 
                return new PaginatedItemsViewModel<CatalogItem>( 
                    pageIndex, pageSize, totalItems, itemsOnPage); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while fetching paginated catalog items asynchronously."); 
                throw; 
            } 
        } 
        public async Task<CatalogItem> FindCatalogItemAsync(int id) 
        { 
            try 
            { 
                _logger.LogInformation("Finding catalog item asynchronously with ID: {Id}", id); 
                return await _db.CatalogItems.Include(c => c.CatalogBrand).Include(c => c.CatalogType).FirstOrDefaultAsync(ci => ci.Id == id); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while finding catalog item asynchronously with ID: {Id}", id); 
                throw; 
            } 
        } 
        public async Task<IEnumerable<CatalogType>> GetCatalogTypesAsync() 
        { 
            try 
            { 
                _logger.LogInformation("Fetching all catalog types asynchronously."); 
                return await _db.CatalogTypes.ToListAsync(); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while fetching catalog types asynchronously."); 
                throw; 
            } 
        } 
        public async Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync() 
        { 
            try 
            { 
                _logger.LogInformation("Fetching all catalog brands asynchronously."); 
                return await _db.CatalogBrands.ToListAsync(); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while fetching catalog brands asynchronously."); 
                throw; 
            } 
        } 
        public async Task CreateCatalogItemAsync(CatalogItem catalogItem) 
        { 
            try 
            { 
                _logger.LogInformation("Creating new catalog item asynchronously: {CatalogItemName}", catalogItem.Name); 
                catalogItem.Id = (await _indexGenerator.GetNextSequenceValueAsync(_db)); 
                _db.CatalogItems.Add(catalogItem); 
                await _db.SaveChangesAsync(); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while creating catalog item asynchronously: {CatalogItemName}", catalogItem.Name); 
                throw; 
            } 
        } 
        public async Task UpdateCatalogItemAsync(CatalogItem catalogItem) 
        { 
            try 
            { 
                _logger.LogInformation("Updating catalog item asynchronously with ID: {Id}", catalogItem.Id); 
                _db.Entry(catalogItem).State = EntityState.Modified; 
                await _db.SaveChangesAsync(); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while updating catalog item asynchronously with ID: {Id}", catalogItem.Id); 
                throw; 
            } 
        } 
        public async Task RemoveCatalogItemAsync(CatalogItem catalogItem) 
        { 
            try 
            { 
                _logger.LogInformation("Removing catalog item asynchronously with ID: {Id}", catalogItem.Id); 
                _db.CatalogItems.Remove(catalogItem); 
                await _db.SaveChangesAsync(); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while removing catalog item asynchronously with ID: {Id}", catalogItem.Id); 
                throw; 
            } 
        } 
        public void Dispose() 
        { 
            _logger.LogInformation("Disposing CatalogService."); 
            _db.Dispose(); 
        } 
    } 
}
