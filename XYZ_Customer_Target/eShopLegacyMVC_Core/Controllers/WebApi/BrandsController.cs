using Microsoft.AspNetCore.Mvc; 
using eShopLegacyMVC_Core.Models; 
using eShopLegacyMVC_Core.Services; 
using Microsoft.Extensions.Logging; 
using System.Collections.Generic; 
using System.Linq; 
using System.Net; 
namespace eShopLegacyMVC_Core.Controllers.WebApi 
{ 
    [Route("api/[controller]")] 
    [ApiController] 
    public class BrandsController : ControllerBase 
    { 
        private readonly ILogger<BrandsController> _logger; 
        private readonly ICatalogService _service; 
        public BrandsController(ILogger<BrandsController> logger, ICatalogService service) 
        { 
            _logger = logger; 
            _service = service; 
        } 
        // GET: api/brands 
        [HttpGet] 
        public ActionResult<IEnumerable<CatalogBrand>> Get() 
        { 
            _logger.LogInformation("Fetching all brands"); 
            var brands = _service.GetCatalogBrandsAsync(); 
            return Ok(brands); 
        } 
        // GET: api/brands/5 
        [HttpGet("{id}")] 
        public async Task<ActionResult<CatalogBrand>> Get(int id) 
        { 
            _logger.LogInformation("Fetching brand with ID: {Id}", id); 
            var brand = (await _service.GetCatalogBrandsAsync()).FirstOrDefault(x => x.Id == id); 
            if (brand == null) 
            { 
                _logger.LogWarning("Brand with ID: {Id} not found", id); 
                return NotFound(); 
            } 
            return Ok(brand); 
        } 
        // DELETE: api/brands/5 
        [HttpDelete("{id}")] 
        public async Task<IActionResult> Delete(int id) 
        { 
            _logger.LogInformation("Attempting to delete brand with ID: {Id}", id); 
            var brandToDelete =(await _service.GetCatalogBrandsAsync()).FirstOrDefault(x => x.Id == id); 
            if (brandToDelete == null) 
            { 
                _logger.LogWarning("Brand with ID: {Id} not found for deletion", id); 
                return NotFound(); 
            } 
            // Demo only - don't actually delete 
            _logger.LogInformation("Brand with ID: {Id} would be deleted (demo only)", id); 
            return Ok(); 
        } 
    } 
} 