using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 
using eShopLegacyMVC_Core.Models; 
using eShopLegacyMVC_Core.Models.Infrastructure; 
using eShopLegacyMVC_Core.ViewModel; 
using Microsoft.Extensions.Logging; 
namespace eShopLegacyMVC_Core.Services 
{ 
    public class CatalogServiceMock : ICatalogService 
    { 
        private readonly List<CatalogItem> _catalogItems; 
        private readonly ILogger<CatalogServiceMock> _logger; 
        public CatalogServiceMock(ILogger<CatalogServiceMock> logger) 
        { 
            _catalogItems = new List<CatalogItem>(PreconfiguredData.GetPreconfiguredCatalogItems()); 
            _logger = logger; 
        } 
        public async Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItemsPaginatedAsync(int pageSize = 10, int pageIndex = 0) 
        { 
            try 
            { 
                _logger.LogInformation("Fetching paginated catalog items asynchronously: pageSize={PageSize}, pageIndex={PageIndex}", pageSize, pageIndex); 
                var items = ComposeCatalogItems(_catalogItems); 
                var itemsOnPage = items 
                    .OrderBy(c => c.Id) 
                    .Skip(pageSize * pageIndex) 
                    .Take(pageSize) 
                    .ToList(); 
                return await Task.FromResult(new PaginatedItemsViewModel<CatalogItem>( 
                    pageIndex, pageSize, items.Count, itemsOnPage)); 
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
                return await Task.FromResult(_catalogItems.FirstOrDefault(x => x.Id == id)); 
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
                return await Task.FromResult(PreconfiguredData.GetPreconfiguredCatalogTypes()); 
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
                return await Task.FromResult(PreconfiguredData.GetPreconfiguredCatalogBrands()); 
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
                var maxId = _catalogItems.Max(i => i.Id); 
                catalogItem.Id = ++maxId; 
                _catalogItems.Add(catalogItem); 
                await Task.CompletedTask; 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while creating catalog item asynchronously: {CatalogItemName}", catalogItem.Name); 
                throw; 
            } 
        } 
        public async Task UpdateCatalogItemAsync(CatalogItem modifiedItem) 
        { 
            try 
            { 
                _logger.LogInformation("Updating catalog item asynchronously with ID: {Id}", modifiedItem.Id); 
                var originalItem = await FindCatalogItemAsync(modifiedItem.Id); 
                if (originalItem != null) 
                { 
                    _catalogItems[_catalogItems.IndexOf(originalItem)] = modifiedItem; 
                } 
                await Task.CompletedTask; 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while updating catalog item asynchronously with ID: {Id}", modifiedItem.Id); 
                throw; 
            } 
        } 
        public async Task RemoveCatalogItemAsync(CatalogItem catalogItem) 
        { 
            try 
            { 
                _logger.LogInformation("Removing catalog item asynchronously with ID: {Id}", catalogItem.Id); 
                _catalogItems.Remove(catalogItem); 
                await Task.CompletedTask; 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while removing catalog item asynchronously with ID: {Id}", catalogItem.Id); 
                throw; 
            } 
        } 
        public void Dispose() 
        { 
            _logger.LogInformation("Disposing CatalogServiceMock."); 
        } 
        private List<CatalogItem> ComposeCatalogItems(List<CatalogItem> items) 
        { 
            var catalogTypes = PreconfiguredData.GetPreconfiguredCatalogTypes(); 
            var catalogBrands = PreconfiguredData.GetPreconfiguredCatalogBrands(); 
            items.ForEach(i => i.CatalogBrand = catalogBrands.First(b => b.Id == i.CatalogBrandId)); 
            items.ForEach(i => i.CatalogType = catalogTypes.First(b => b.Id == i.CatalogTypeId)); 
            return items; 
        } 
    } 
}