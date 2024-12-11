using System.Collections.Generic; 
using System.Threading.Tasks; 
using eShopLegacyMVC_Core.Models; 
using System; 
using eShopLegacyMVC_Core.ViewModel; 
namespace eShopLegacyMVC_Core.Services 
{ 
    public interface ICatalogService : IDisposable 
    { 
        Task<CatalogItem> FindCatalogItemAsync(int id); 
        Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync(); 
        Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItemsPaginatedAsync(int pageSize, int pageIndex); 
        Task<IEnumerable<CatalogType>> GetCatalogTypesAsync(); 
        Task CreateCatalogItemAsync(CatalogItem catalogItem); 
        Task UpdateCatalogItemAsync(CatalogItem catalogItem); 
        Task RemoveCatalogItemAsync(CatalogItem catalogItem); 
    } 
}