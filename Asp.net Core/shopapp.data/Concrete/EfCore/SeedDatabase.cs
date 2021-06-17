using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed(){

            var context = new ShopContext();

            if(context.Database.GetPendingMigrations().Count()==0){ // eklenecek migrations yoksa 

                if(context.Categories.Count() == 0){ // categories tablosunun içi boşsa 

                    context.Categories.AddRange(Categories);
                }

                if(context.Products.Count() == 0){ // Products tablosunun içi boşsa 

                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }

            }

            context.SaveChanges();
        }

        private static Category[] Categories = {
            new Category(){Name="Telefon",Url="telefon"},
            new Category(){Name="Bilgisayar",Url="bilgisayar"},
            new Category(){Name="Elektronik",Url="elektronik"},
            new Category(){Name="Beyaz Eşya",Url="beyaz-esya"}
        };


        private static Product[] Products = {
            new Product(){Name="Iphone 7",Url="Iphone-7",Price=3000,Description="Güzel telefon",ImageUrl="4.jpg",IsApproved=true},
            new Product(){Name="Iphone 8",Url="Iphone-8",Price=4000,Description="Güzel telefon",ImageUrl="4.jpg",IsApproved=false},
            new Product(){Name="Iphone X",Url="Iphone-X",Price=5000,Description="Güzel telefon",ImageUrl="3.jpg",IsApproved=false},
            new Product(){Name="Iphone 11",Url="Iphone-11",Price=6000,Description="Güzel telefon",ImageUrl="3.jpg",IsApproved=true},
            new Product(){Name="Iphone 12",Url="Iphone-12",Price=7000,Description="Güzel telefon",ImageUrl="1.jpg",IsApproved=true},
        };


        private static ProductCategory[] ProductCategories = {
            
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[0],Category=Categories[2]},
            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[2]},
            new ProductCategory(){Product=Products[2],Category=Categories[0]},
            new ProductCategory(){Product=Products[2],Category=Categories[2]},
            new ProductCategory(){Product=Products[3],Category=Categories[0]},
            new ProductCategory(){Product=Products[3],Category=Categories[2]}
           
             
        };
    }
}