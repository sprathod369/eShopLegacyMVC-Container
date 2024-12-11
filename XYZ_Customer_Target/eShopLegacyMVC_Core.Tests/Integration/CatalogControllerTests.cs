using System.Threading.Tasks; 
using Xunit; 
using Microsoft.AspNetCore.Mvc.Testing; 
using eShopLegacyMVC_Core; 
using System.Net.Http; 
using System.Net; 
using Newtonsoft.Json; 
using eShopLegacyMVC_Core.ViewModel; 
using eShopLegacyMVC_Core.Models; 
using Microsoft.Extensions.Logging; 
using Microsoft.Extensions.DependencyInjection; 
namespace eShopLegacyMVC_Core.Tests.Integration 
{ 
    public class CatalogControllerTests : IClassFixture<WebApplicationFactory<Startup>> 
    { 
        private readonly HttpClient _client; 
        private readonly ILogger<CatalogControllerTests> _logger; 
        public CatalogControllerTests(WebApplicationFactory<Startup> factory) 
        { 
            _client = factory.CreateClient(); 
            var serviceProvider = factory.Services; 
            _logger = serviceProvider.GetRequiredService<ILogger<CatalogControllerTests>>(); 
        } 
        [Fact] 
        public async Task Get_Index_ShouldReturnSuccessAndCorrectContentType() 
        { 
            _logger.LogInformation("Testing Get_Index_ShouldReturnSuccessAndCorrectContentType"); 
            // Act 
            var response = await _client.GetAsync("/Catalog/Index"); 
            // Assert 
            response.EnsureSuccessStatusCode(); 
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString()); 
        } 
        [Fact] 
        public async Task Get_CatalogItems_ShouldReturnPaginatedItems() 
        { 
            _logger.LogInformation("Testing Get_CatalogItems_ShouldReturnPaginatedItems"); 
            // Act 
            var response = await _client.GetAsync("/Catalog/Index?pageSize=10&pageIndex=0"); 
            var content = await response.Content.ReadAsStringAsync(); 
            var result = JsonConvert.DeserializeObject<PaginatedItemsViewModel<CatalogItem>>(content); 
            // Assert 
            Assert.NotNull(result); 
            Assert.True(result.TotalItems > 0); 
        } 
        // Additional integration tests... 
    } 
}