    using Microsoft.AspNetCore.Mvc; 
    using eShopLegacyMVC_Core.Services; 
    using Microsoft.Extensions.Logging; 
    using System; 
    using System.Collections.Generic; 
    using System.Linq; 
    using System.Net; 
    namespace eShopLegacyMVC_Core.Controllers.WebApi 
    { 
        [Route("api/[controller]")] 
        [ApiController] 
        public class FilesController : ControllerBase 
        { 
            private readonly ILogger<FilesController> _logger; 
            private readonly ICatalogService _service; 
            public FilesController(ILogger<FilesController> logger, ICatalogService service) 
            { 
                _logger = logger; 
                _service = service; 
            } 
            // GET: api/files 
            [HttpGet] 
            public async Task<IActionResult> Get() 
            { 
                try 
                { 
                    _logger.LogInformation("Fetching all brands as binary stream"); 
                    var brands = (await _service.GetCatalogBrandsAsync()) 
                        .Select(b => new BrandDTO 
                        { 
                            Id = b.Id, 
                            Brand = b.Brand 
                        }).ToList(); 
                    var serializer = new Serializing(); 
                    var stream = serializer.SerializeBinary(brands); 
                    return File(stream, "application/octet-stream"); 
                } 
                catch (Exception ex) 
                { 
                    _logger.LogError(ex, "An error occurred while fetching brands"); 
                    return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request."); 
                } 
            } 
            [Serializable] 
            public class BrandDTO 
            { 
                public int Id { get; set; } 
                public string Brand { get; set; } 
            } 
        } 
    }