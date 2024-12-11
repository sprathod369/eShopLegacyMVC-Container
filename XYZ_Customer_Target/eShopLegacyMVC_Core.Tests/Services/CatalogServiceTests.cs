using System; 
using System.Threading.Tasks; 
using Xunit; 
using Moq; 
using eShopLegacyMVC_Core.Services; 
using eShopLegacyMVC_Core.Models; 
using Microsoft.Extensions.Logging; 
using System.Collections.Generic; 
using System.Linq; 
namespace eShopLegacyMVC_Core.Tests.Services 
{ 
    public class CatalogServiceTests 
    { 
        private readonly Mock<CatalogDBContext> _dbContextMock; 
        private readonly Mock<CatalogItemHiLoGenerator> _indexGeneratorMock; 
        private readonly Mock<ILogger<CatalogService>> _loggerMock; 
        private readonly CatalogService _catalogService; 
        public CatalogServiceTests() 
        { 
            _dbContextMock = new Mock<CatalogDBContext>(); 
            _indexGeneratorMock = new Mock<CatalogItemHiLoGenerator>(); 
            _loggerMock = new Mock<ILogger<CatalogService>>(); 
            _catalogService = new CatalogService(_dbContextMock.Object, _indexGeneratorMock.Object, _loggerMock.Object); 
        } 
        [Fact] 
        public async Task GetCatalogItemsPaginatedAsync_ShouldReturnItems() 
        { 
            // Arrange 
            var items = new List<CatalogItem> 
            { 
                new CatalogItem { Id = 1, Name = "Item 1" }, 
                new CatalogItem { Id = 2, Name = "Item 2" } 
            }.AsQueryable(); 
            _dbContextMock.Setup(db => db.CatalogItems).ReturnsDbSet(items); 
            // Act 
            var result = await _catalogService.GetCatalogItemsPaginatedAsync(10, 0); 
            // Assert 
            Assert.Equal(2, result.Data.Count()); 
        } 
        [Fact] 
        public async Task FindCatalogItemAsync_ShouldReturnItem_WhenItemExists() 
        { 
            // Arrange 
            var item = new CatalogItem { Id = 1, Name = "Item 1" }; 
            _dbContextMock.Setup(db => db.CatalogItems.FindAsync(1)).ReturnsAsync(item); 
            // Act 
            var result = await _catalogService.FindCatalogItemAsync(1); 
            // Assert 
            Assert.NotNull(result); 
            Assert.Equal("Item 1", result.Name); 
        } 
        [Fact] 
        public async Task FindCatalogItemAsync_ShouldReturnNull_WhenItemDoesNotExist() 
        { 
            // Arrange 
            _dbContextMock.Setup(db => db.CatalogItems.FindAsync(1)).ReturnsAsync((CatalogItem)null); 
            // Act 
            var result = await _catalogService.FindCatalogItemAsync(1); 
            // Assert 
            Assert.Null(result); 
        } 
        [Fact] 
        public async Task CreateCatalogItemAsync_ShouldAddItem() 
        { 
            // Arrange 
            var item = new CatalogItem { Name = "New Item" }; 
            _indexGeneratorMock.Setup(gen => gen.GetNextSequenceValueAsync(_dbContextMock.Object)).ReturnsAsync(1); 
            // Act 
            await _catalogService.CreateCatalogItemAsync(item); 
            // Assert 
            _dbContextMock.Verify(db => db.CatalogItems.Add(item), Times.Once); 
            _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once); 
        } 
        [Fact] 
        public async Task UpdateCatalogItemAsync_ShouldModifyItem() 
        { 
            // Arrange 
            var item = new CatalogItem { Id = 1, Name = "Updated Item" }; 
            _dbContextMock.Setup(db => db.CatalogItems.Update(item)); 
            // Act 
            await _catalogService.UpdateCatalogItemAsync(item); 
            // Assert 
            _dbContextMock.Verify(db => db.CatalogItems.Update(item), Times.Once); 
            _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once); 
        } 
        [Fact] 
        public async Task RemoveCatalogItemAsync_ShouldDeleteItem() 
        { 
            // Arrange 
            var item = new CatalogItem { Id = 1, Name = "Item to Delete" }; 
            _dbContextMock.Setup(db => db.CatalogItems.Remove(item)); 
            // Act 
            await _catalogService.RemoveCatalogItemAsync(item); 
            // Assert 
            _dbContextMock.Verify(db => db.CatalogItems.Remove(item), Times.Once); 
            _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once); 
        } 
    } 
}