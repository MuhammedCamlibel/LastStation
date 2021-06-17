using System.Collections.Generic;

namespace shopapp.entity
{
    public class Product
    {
         public int ProductId { get; set; }
        
        public string Name  { get; set; }
        
        public string Url  { get; set; }
        
        
        public double? Price { get; set; } // fiyat bilgisi girmezse null deger atanıcak ? koymasaydık 0 degeri atanırdı

        public string Description { get; set; }

         public string ImageUrl { get; set; }
        
        public bool IsApproved { get; set; }
        
        public bool IsHome { get; set; }
        
        public List<ProductCategory> ProductCategories { get; set; } 
       
    }
}