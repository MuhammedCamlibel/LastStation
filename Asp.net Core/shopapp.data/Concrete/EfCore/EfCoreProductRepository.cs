using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public Product GetByIdWithCategories(int id)
        {
            using(var context = new ShopContext()){
                return context.Products.Where(i=>i.ProductId==id)
                                              .Include(i=>i.ProductCategories)
                                              .ThenInclude(i=>i.Category)
                                              .FirstOrDefault();
                                                 
            }
        }

        public int GetCountByCategory(string category)
        {
           using(var context = new ShopContext()){
                
                var products = context.Products.Where(i=>i.IsApproved).AsQueryable(); // veritabanında sorgulama yapılmaz sorgu üzerinde çalışmak için (where ekleyerek IsApproved alanı 1 olanları çagırması ve onları saymasını sağladık)

                if(!string.IsNullOrEmpty(category)){
                    
                    products = products
                                   .Include(i=>i.ProductCategories) // bilgileri almak için Tabloya gittik
                                   .ThenInclude(i=>i.Category) // yukardaki tablodan başka bir tabloya gittik (bilgileri almak için join )
                                   .Where(i=>i.ProductCategories.Any(a=>a.Category.Url == category)); // category name yolladımız name aymı olanları alır Productcategories tablosundan
                }

                return products.Count(); // veritabanı yukarda belirtilen degerlere göre sorgulanır 
            }
        }

        public List<Product> GetHomePageProducts()
        {
            using(var context = new ShopContext()){

                return context.Products.Where(i=>i.IsHome==true && i.IsApproved).ToList();
            }
        }

       

        public Product GetProductDetails(string url)
        {
            using(var context = new ShopContext()){

                    return context.Products
                                        .Where(i=>i.Url==url)  // Products tablosundan ProductId uyan elemanı bulduk
                                        .Include(i=>i.ProductCategories) // ProductCategories tablosuna gittik   (join işlemi)
                                        .ThenInclude(i=>i.Category)  // ProductCategories içinden Categori tablosuna gittik  (join işlemi)
                                        .FirstOrDefault(); // ilgili ProductId uyan kayıt varsa ilk kaydı getir dedik 

            }
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            using(var context = new ShopContext()){
                
                var products = context.Products.Where(i=>i.IsApproved).AsQueryable(); // veritabanında sorgulama yapılmaz sorgu üzerinde çalışmak için (where getirerek IsApproved alanı dogru alanları getirmesini söyledik)

                if(!string.IsNullOrEmpty(name)){
                    
                    products = products
                                   .Include(i=>i.ProductCategories) // bilgileri almak için Tabloya gittik
                                   .ThenInclude(i=>i.Category) // yukardaki tablodan başka bir tabloya gittik (bilgileri almak için join )
                                   .Where(i=>i.ProductCategories.Any(a=>a.Category.Url == name)); // category name yolladımız name aymı olanları alır Productcategories tablosundan
                }

                return products.Skip((page-1)*pageSize).Take(pageSize).ToList(); // veritabanı yukarda belirtilen degerlere göre sorgulanır 
            }
        }

        public List<Product> GetSearchResult(string searchString,string fiyat)
        {
            using(var context = new ShopContext()){

               if(searchString == null && fiyat == null)
               {
                   return context.Products.ToList();
               }

               if(searchString == null && fiyat != null)
               {
                   return context.Products.OrderBy(i=>i.Price).ToList();
               } 
                
                var products = context.Products.Where(i=>i.IsApproved && (i.Name.ToUpper().Contains(searchString.ToUpper()) || i.Description.ToUpper().Contains(searchString.ToUpper()))).AsQueryable(); // veritabanında sorgulama yapılmaz sorgu üzerinde çalışmak için (where ekleyerek IsApproved alanı 1 olanları çagırması ve onları saymasını sağladık)

                

                if(!string.IsNullOrEmpty(fiyat))
                {
                   return products.OrderBy(p=>p.Price).ToList();
                }

                return products.ToList(); // veritabanı yukarda belirtilen degerlere göre sorgulanır 
            }
        }

        public List<Product> GetTop5Products()
        {
            throw new System.NotImplementedException();
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using(var context = new ShopContext()){

                var product = context.Products
                                       .Include(i=>i.ProductCategories) // ProductCategories tablosunu getirdik 
                                       .FirstOrDefault(i=>i.ProductId == entity.ProductId);

                if(product != null){

                    product.Name = entity.Name;
                    product.Price = entity.Price;
                    product.Description = entity.Description;
                    product.Url = entity.Url;
                    product.ImageUrl = entity.ImageUrl;
                    product.IsApproved = entity.IsApproved;
                    product.IsHome = entity.IsHome;

                    product.ProductCategories = categoryIds.Select(catid=>new ProductCategory() // herbir categori id si için bu nesne üreticek
                    {
                        ProductId = entity.ProductId,
                        CategoryId = catid

                    }).ToList();

                    context.SaveChanges();
                }                       
            }
        }
    }
}