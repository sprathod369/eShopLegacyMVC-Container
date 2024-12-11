using Microsoft.AspNetCore.Mvc; 
using eShopLegacyMVC_Core.Models; 
using eShopLegacyMVC_Core.Services; 
using Microsoft.Extensions.Logging; 
using System.IO; 
namespace eShopLegacyMVC_Core.Controllers 
{ 
    public class PicController : Controller 
    { 
        private readonly ILogger<PicController> _logger; 
        private readonly ICatalogService _service; 
        public const string GetPicRouteName = "GetPicRouteTemplate"; 
        public PicController(ILogger<PicController> logger, ICatalogService service) 
        { 
            _logger = logger; 
            _service = service; 
        } 
        // GET: items/{catalogItemId:int}/pic 
        [HttpGet] 
        [Route("items/{catalogItemId:int}/pic", Name = GetPicRouteName)] 
        public async Task<IActionResult> Index(int catalogItemId) 
        { 
            _logger.LogInformation("Now loading... /items/{CatalogItemId}/pic", catalogItemId); 
            if (catalogItemId <= 0) 
            { 
                return BadRequest(); 
            } 
            var item = await _service.FindCatalogItemAsync(catalogItemId); 
            if (item != null) 
            { 
                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Pics"); 
                var path = Path.Combine(webRoot, item.PictureFileName); 
                if (!System.IO.File.Exists(path)) 
                { 
                    _logger.LogError("File not found: {Path}", path); 
                    return NotFound(); 
                } 
                string imageFileExtension = Path.GetExtension(item.PictureFileName); 
                string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension); 
                var buffer = System.IO.File.ReadAllBytes(path); 
                return File(buffer, mimetype); 
            } 
            return NotFound(); 
        } 
        private string GetImageMimeTypeFromImageFileExtension(string extension) 
        { 
            return extension.ToLower() switch 
            { 
                ".png" => "image/png", 
                ".gif" => "image/gif", 
                ".jpg" => "image/jpeg", 
                ".jpeg" => "image/jpeg", 
                ".bmp" => "image/bmp", 
                ".tiff" => "image/tiff", 
                ".wmf" => "image/wmf", 
                ".jp2" => "image/jp2", 
                ".svg" => "image/svg+xml", 
                _ => "application/octet-stream", 
            }; 
        } 
    } 
} 