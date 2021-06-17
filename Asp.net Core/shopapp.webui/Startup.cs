using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using shopapp.business.Abstract;
using shopapp.business.Concrete;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.webui.EmailServices;
using shopapp.webui.Identity;

namespace shopapp.webui
{
    public class Startup
    {   
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options=>options.UseSqlite("Data Source=shopDb")); // kullanıcamız veritabanını verdik
            services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders(); // Identity projemizde tanımladık (buradaki token şifre resetleme gibi işler için giriş yaparken verilen token degil)           

            services.Configure<IdentityOptions>(options=> {  // AddIdentity altında olucak
                  // password 
                  options.Password.RequireDigit = true; // şifrede sayısal deger olucak
                  options.Password.RequireUppercase = true; // büyük harf olucak
                  options.Password.RequireLowercase = true;
                  options.Password.RequiredLength = 6; // en az 6 karakter 
                  options.Password.RequireNonAlphanumeric = true; // _ - gibi alfanumerik karakterlerin olmasını istiyoruz

                  // Lockout 

                  options.Lockout.MaxFailedAccessAttempts = 5; // en fazla 5 kere yanlış şifre girebilir
                  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // bloke etme süresi 5 dk
                  options.Lockout.AllowedForNewUsers = true; // her yeni kullanıcı için kurallar geçerli 

                  // User 

                  options.User.RequireUniqueEmail = true; // benzersiz bir email
                   
                  // SignIn 
                  options.SignIn.RequireConfirmedEmail = true; // email onay kısmı 
                  options.SignIn.RequireConfirmedPhoneNumber = false;   // telefon onay kısmı  
            });

            services.ConfigureApplicationCookie(options=> { // AddIdentity altında olucak

                  options.LoginPath = "/account/login"; // üye girişine yönlendirecemiz sayfa  
                  options.LogoutPath = "/account/logout"; // çıkış yaptımızda gideceğimiz sayfa 
                  options.AccessDeniedPath = "/account/accessdenied"; // erişim olmayan yerlerde gösterilecek sayfa 
                  options.SlidingExpiration = true; // hesabın belirli bir dk sonra cookie tanımaması
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // belirli dk 
                  options.Cookie = new CookieBuilder  
                  {
                      HttpOnly = true, // sadece tarayıcıdan cookie bilgisi alınsın 
                      Name = ".ShopApp.Security.Cookie", // Cookie adı
                      SameSite = SameSiteMode.Strict // sadece kullanıcının bilgisayarından 
                  }; 
            });
 

            services.AddScoped<IProductRepository,EfCoreProductRepository>();  // interface çagırıldıgında dolu olan sınıfı getirilcek
            services.AddScoped<IProductService,ProductManager>(); // IProductService çagırdıgımda ProductManager gelecek
           
            services.AddScoped<ICategoryRepository,EfCoreCategoryRepository>();
            services.AddScoped<ICategoryService,CategoryManager>();

            services.AddScoped<ICartRepository,EfCoreCartRepository>();
            services.AddScoped<ICartService,CartManager>();

            services.AddScoped<IContactRepository,EfCoreContactRepository>();
            services.AddScoped<IContactService,ContactManager>();
            
            services.AddScoped<IOrderRepository,EfCoreOrderRepository>();
            services.AddScoped<IOrderService,OrderManager>();


            services.AddScoped<IEmailSender,SmtpEmailSender>(i=>
                 
                 new SmtpEmailSender(
                     _configuration["EmailSender:Host"],
                     _configuration.GetValue<int>("EmailSender:Port"),
                     _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                     _configuration["EmailSender:Username"],
                      _configuration["EmailSender:Password"]
                 )
            
            );
            services.AddControllersWithViews(); // mvc ile çalışacağımızı söyledik
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration configuration, UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {

            app.UseStaticFiles(); // wwwroot altındaki klasörler açılmış olur 

            app.UseStaticFiles(new StaticFileOptions{     // farklı bir static klasör ekleme 
                 FileProvider = new PhysicalFileProvider(
                     Path.Combine(Directory.GetCurrentDirectory(),"node_modules")), // ekleyecegimiz uzantı 
                     RequestPath = "/modules" // hangi isimle çagıracamız 
                   
            });

            if (env.IsDevelopment())
            {
                SeedDatabase.Seed(); // uygulama  geliştirme aşamasında isek bu static metodu çagıracak (veritabanını doldurmak için)
                app.UseDeveloperExceptionPage();
            }


            app.UseAuthentication(); // Identity servis yapısını ekledik

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>   
            {
               endpoints.MapControllerRoute(
                     name : "orders",
                     pattern : "orders",
                     defaults : new{controller="Cart",action="GetOrders"}   
               ); 

               endpoints.MapControllerRoute(
                     name : "checkout",
                     pattern : "checkout",
                     defaults : new{controller="Cart",action="Checkout"}   
               );

               endpoints.MapControllerRoute(
                     name : "cart",
                     pattern : "cart",
                     defaults : new{controller="Cart",action="Index"}   
               );
               
               endpoints.MapControllerRoute(
                                          
                     name: "adminusermanage",
                     pattern: "admin/user/manage", // url yazılacak
                     defaults:  new{controller="Admin",action="UserManage"} // gidecegi yer   
 
                );

               endpoints.MapControllerRoute(
                                          
                     name: "adminuserdelete",
                     pattern: "admin/user/delete", // url yazılacak
                     defaults:  new{controller="Admin",action="UserDelete"} // gidecegi yer   
 
                ); 


               endpoints.MapControllerRoute(
                                          
                     name: "adminuseredit",
                     pattern: "admin/user/{id?}", // url yazılacak
                     defaults:  new{controller="Admin",action="UserEdit"} // gidecegi yer   
 
                ); 

                endpoints.MapControllerRoute(
                                          
                     name: "adminusers",
                     pattern: "admin/user/list", // url yazılacak
                     defaults:  new{controller="Admin",action="UserList"} // gidecegi yer   
 
                );  

                endpoints.MapControllerRoute(
                                          
                     name: "adminroles",
                     pattern: "admin/role/list", // url yazılacak
                     defaults:  new{controller="Admin",action="RoleList"} // gidecegi yer   
 
                );

                endpoints.MapControllerRoute(
                                          
                     name: "adminrolecreate",
                     pattern: "admin/role/create", // url yazılacak
                     defaults:  new{controller="Admin",action="RoleCreate"} // gidecegi yer   
 
                );

                endpoints.MapControllerRoute(
                                          
                     name: "adminroleedit",
                     pattern: "admin/role/{id?}", // url yazılacak
                     defaults:  new{controller="Admin",action="RoleEdit"} // gidecegi yer   
 
                );
                
                endpoints.MapControllerRoute(
                     // sabit olaral url tanımladık                      
                     name: "adminproducts",
                     pattern: "admin/products", // url yazılacak
                     defaults:  new{controller="Admin",action="ProductList"} // gidecegi yer   
 
                );

                endpoints.MapControllerRoute(
                     // sabit olaral url tanımladık                      
                     name: "adminproductscreate",
                     pattern: "admin/products/create", // url yazılacak
                     defaults:  new{controller="Admin",action="ProductCreate"} // gidecegi yer   
 
                );

                 endpoints.MapControllerRoute(
                     // sabit olaral url tanımladık                      
                     name: "adminproductedit",
                     pattern: "admin/products/{id?}", // url yazılacak
                     defaults:  new{controller="Admin",action="ProductEdit"} // gidecegi yer   
 
                );

                endpoints.MapControllerRoute(
                     // sabit olaral url tanımladık                      
                     name: "admincategories",
                     pattern: "admin/categories", // url yazılacak
                     defaults:  new{controller="Admin",action="CategoryList"} // gidecegi yer   
 
                );

                endpoints.MapControllerRoute(
                     // sabit olaral url tanımladık                      
                     name: "admincategoriescreate",
                     pattern: "admin/categories/create", // url yazılacak
                     defaults:  new{controller="Admin",action="CategoryCreate"} // gidecegi yer   
 
                ); 

                endpoints.MapControllerRoute(
                     // sabit olaral url tanımladık                      
                     name: "admincategoryedit",
                     pattern: "admin/categories/{id?}", // url yazılacak
                     defaults:  new{controller="Admin",action="CategoryEdit"} // gidecegi yer   
 
                );

                
               

                endpoints.MapControllerRoute(
                     // sabit olaral url tanımladık                      
                     name: "searchproducts",
                     pattern: "search", // url yazılacak
                     defaults:  new{controller="Shop",action="search"} // gidecegi yer   
 
                ); 

                endpoints.MapControllerRoute(
                     // sabit olaral url tanımladık                      
                     name: "productdetails",
                     pattern: "{url}", // url yazılacak
                     defaults:  new{controller="Shop",action="details"} // gidecegi yer   
 
                );   


                endpoints.MapControllerRoute(
                     // sabit olaral url tanımladık                      
                     name: "products",
                     pattern: "products/{category?}", // url yazılacak
                     defaults:  new{controller="Shop",action="list"} // gidecegi yer   
 
                );                

                endpoints.MapControllerRoute( // yönlendirme işleme için
                    name : "default",
                    pattern : "{controller=Home}/{action=Index}/{id?}"
                );
            });

            SeedIdentity.Seed(userManager,roleManager,configuration).Wait();
        }
    }
}
