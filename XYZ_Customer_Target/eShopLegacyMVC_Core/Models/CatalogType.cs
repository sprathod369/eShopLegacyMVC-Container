using System.ComponentModel.DataAnnotations; 
namespace eShopLegacyMVC_Core.Models 
{ 
    public class CatalogType 
    { 
        public int Id { get; set; } 
        [Required] 
        [StringLength(100)] 
        public string Type { get; set; } 
    } 
} 