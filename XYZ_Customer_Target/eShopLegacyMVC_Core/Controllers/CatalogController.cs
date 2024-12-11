using Microsoft.AspNetCore.Mvc; 
using eShopLegacyMVC_Core.Models; 
using eShopLegacyMVC_Core.Services; 
using Microsoft.Extensions.Logging; 
using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks; 
namespace eShopLegacyMVC_Core.Controllers 
{ 
    public class CatalogController : Controller 
    { 
        private readonly ILogger<CatalogController> _logger; 
        private readonly ICatalogService _service; 
        public CatalogController(ILogger<CatalogController> logger, ICatalogService service) 
        { 
            _logger = logger; 
            _service = service; 
        } 
        public async Task<IActionResult> Index(int pageSize = 10, int pageIndex = 0) 
        { 
            _logger.LogInformation("Loading Catalog Index asynchronously"); 
            var paginatedItems = await _service.GetCatalogItemsPaginatedAsync(pageSize, pageIndex); 
            ChangeUriPlaceholder(paginatedItems.Data); 
            return View(paginatedItems); 
        } 
        public async Task<IActionResult> Details(int? id) 
        { 
            if (id == null) 
            { 
                _logger.LogWarning("Details action called with null id"); 
                return BadRequest(); 
            } 
            var catalogItem = await _service.FindCatalogItemAsync(id.Value); 
            if (catalogItem == null) 
            { 
                _logger.LogWarning("Catalog item not found for id: {Id}", id); 
                return NotFound(); 
            } 
            AddUriPlaceHolder(catalogItem); 
            return View(catalogItem); 
        } 
        public async Task<IActionResult> Create() 
        { 
            ViewBag.CatalogBrandId = new SelectList(await _service.GetCatalogBrandsAsync(), "Id", "Brand"); 
            ViewBag.CatalogTypeId = new SelectList(await _service.GetCatalogTypesAsync(), "Id", "Type"); 
            return View(new CatalogItem()); 
        } 
        [HttpPost] 
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,PictureFileName,CatalogTypeId,CatalogBrandId,AvailableStock,RestockThreshold,MaxStockThreshold,OnReorder")] CatalogItem catalogItem) 
        { 
            if (ModelState.IsValid) 
            { 
                _logger.LogInformation("Creating catalog item asynchronously: {CatalogItemName}", catalogItem.Name); 
                await _service.CreateCatalogItemAsync(catalogItem); 
                return RedirectToAction(nameof(Index)); 
            } 
            _logger.LogWarning("Model state invalid while creating catalog item"); 
            ViewBag.CatalogBrandId = new SelectList(await _service.GetCatalogBrandsAsync(), "Id", "Brand", catalogItem.CatalogBrandId); 
            ViewBag.CatalogTypeId = new SelectList(await _service.GetCatalogTypesAsync(), "Id", "Type", catalogItem.CatalogTypeId); 
            return View(catalogItem); 
        } 
        public async Task<IActionResult> Edit(int? id) 
        { 
            if (id == null) 
            { 
                _logger.LogWarning("Edit action called with null id"); 
                return BadRequest(); 
            } 
            var catalogItem = await _service.FindCatalogItemAsync(id.Value); 
            if (catalogItem == null) 
            { 
                _logger.LogWarning("Catalog item not found for id: {Id}", id); 
                return NotFound(); 
            } 
            AddUriPlaceHolder(catalogItem); 
            ViewBag.CatalogBrandId = new SelectList(await _service.GetCatalogBrandsAsync(), "Id", "Brand", catalogItem.CatalogBrandId); 
            ViewBag.CatalogTypeId = new SelectList(await _service.GetCatalogTypesAsync(), "Id", "Type", catalogItem.CatalogTypeId); 
            return View(catalogItem); 
        } 
        [HttpPost] 
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit([Bind("Id,Name,Description,Price,PictureFileName,CatalogTypeId,CatalogBrandId,AvailableStock,RestockThreshold,MaxStockThreshold,OnReorder")] CatalogItem catalogItem) 
        { 
            if (ModelState.IsValid) 
            { 
                _logger.LogInformation("Updating catalog item asynchronously with ID: {Id}", catalogItem.Id); 
                await _service.UpdateCatalogItemAsync(catalogItem); 
                return RedirectToAction(nameof(Index)); 
            } 
            _logger.LogWarning("Model state invalid while updating catalog item with ID: {Id}", catalogItem.Id); 
            ViewBag.CatalogBrandId = new SelectList(await _service.GetCatalogBrandsAsync(), "Id", "Brand", catalogItem.CatalogBrandId); 
            ViewBag.CatalogTypeId = new SelectList(await _service.GetCatalogTypesAsync(), "Id", "Type", catalogItem.CatalogTypeId); 
            return View(catalogItem); 
        } 
        public async Task<IActionResult> Delete(int? id) 
        { 
            if (id == null) 
            { 
                _logger.LogWarning("Delete action called with null id"); 
                return BadRequest(); 
            } 
            var catalogItem = await _service.FindCatalogItemAsync(id.Value); 
            if (catalogItem == null) 
            { 
                _logger.LogWarning("Catalog item not found for id: {Id}", id); 
                return NotFound(); 
            } 
            AddUriPlaceHolder(catalogItem); 
            return View(catalogItem); 
        } 
        [HttpPost, ActionName("Delete")] 
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> DeleteConfirmed(int id) 
        { 
            _logger.LogInformation("Deleting catalog item asynchronously with ID: {Id}", id); 
            var catalogItem = await _service.FindCatalogItemAsync(id); 
            if (catalogItem == null) 
            { 
                _logger.LogWarning("Catalog item not found for id: {Id} during delete confirmation", id); 
                return NotFound(); 
            } 
            await _service.RemoveCatalogItemAsync(catalogItem); 
            return RedirectToAction(nameof(Index)); 
        } 
        protected override void Dispose(bool disposing) 
        { 
            _logger.LogDebug("Now disposing"); 
            if (disposing) 
            { 
                _service.Dispose(); 
            } 
            base.Dispose(disposing); 
        } 
        private void ChangeUriPlaceholder(IEnumerable<CatalogItem> items) 
        { 
            foreach (var catalogItem in items) 
            { 
                AddUriPlaceHolder(catalogItem); 
            } 
        } 
        private void AddUriPlaceHolder(CatalogItem item) 
        { 
            item.PictureUri = Url.Action("Index", "Pic", new { catalogItemId = item.Id }, Request.Scheme); 
        } 
    } 
}