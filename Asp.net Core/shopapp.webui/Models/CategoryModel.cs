using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage="Name zorunlu bir alan")]
        [StringLength(60,MinimumLength=5,ErrorMessage="Kategori ismi 5-60 karakterden uzunlugundan olmalıdır")]
        public string Name { get; set; }
        
        [Required(ErrorMessage="Url zorunlu bir alan")]
        [StringLength(60,MinimumLength=5,ErrorMessage="Url ismi 5-60 karakter uzunlugunda olmalıdır")]
        public string Url { get; set; }

        public List<Product> Products { get; set; }  // GetByIdWithProducts() metodundan product larıda çektimiz için bu prop u koyduk
 
        
    }
}