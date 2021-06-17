using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{

    [Authorize]  //  ilgili metotlara giderken kişi kontrolü edilecegini söyledik
    //[Authorize(Roles="Admin")]
    public class AdminController:Controller
    {
        private IProductService _productService;

        private ICategoryService _categoryService;

        private RoleManager<IdentityRole> _roleManager;

        private UserManager<User> _userManager;

        public AdminController(IProductService productService,ICategoryService categoryService,RoleManager<IdentityRole> roleManager,UserManager<User> userManager)
        {
            _productService = productService;

            _categoryService = categoryService;

            _roleManager = roleManager;

            _userManager = userManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> UserManage(string Name)
        {   
            var user = await _userManager.GetUserAsync(User); // oturum açmış kullanıcıyı alma 
            //var user = await _userManager.FindByNameAsync(Name);

            
            if(user!=null)
            {  
               return View(new UserManageModel(){

                   UserId = user.Id,
                   UserName = user.UserName,
                   FirstName = user.FirstName,
                   LastName = user.LastName,
                   Email = user.Email
                   
               });
            }
            
            return View();
           
        }
        
        [HttpPost]
        public async Task<IActionResult> UserManage(UserManageModel model)
        {
           var user = await _userManager.FindByIdAsync(model.UserId);
           if(user!=null)
           {
               user.UserName = model.UserName;
               user.FirstName = model.FirstName;
               user.LastName = model.LastName;
               user.Email = model.Email;

               var result = await _userManager.UpdateAsync(user);
               
               if(result.Succeeded)
               {
                   return Redirect("/Home/Index");
               }

               
           }
           return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> UserDelete(string UserId)
        {
              var user = await _userManager.FindByIdAsync(UserId);

              if(user!=null)
              {
                  var result = await _userManager.DeleteAsync(user);
                  if(result.Succeeded)
                  {
                      return RedirectToAction("UserList");
                  }
              }
              return View();
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
             
            if(user!=null)
            {
               var selectedRoles = await _userManager.GetRolesAsync(user);
               var roles = _roleManager.Roles.Select(i=>i.Name);

               ViewBag.Roles = roles;

               return View(new UserDetailsModel(){
                   UserId = user.Id,
                   UserName = user.UserName,
                   FirstName = user.FirstName,
                   LastName = user.LastName,
                   Email = user.Email,
                   EmailConfirmed = user.EmailConfirmed,
                   SelectedRoles = selectedRoles 
                   
               });
            }
            return Redirect("~/admin/user/list");
        }

        [HttpPost] 
        public async Task<IActionResult> UserEdit(UserDetailsModel model,string[] selectedRoles)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if(user!=null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.EmailConfirmed = model.EmailConfirmed;
                    user.Email = model.Email;

                    var result = await _userManager.UpdateAsync(user);

                    if(result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles?? new string[]{}; // eger selectedRoles boş gelmişse boş bir string dizisi tanımlıyoruz yoksa nullreferance hatası verir 
                        await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles).ToArray<string>()); // seçtigi rolleri veritabanında olmayanlarla karşılaştırarak olmayanları ekler. (except hariç demek. olanları ekleme dedik)
                        await _userManager.RemoveFromRolesAsync(user,userRoles.Except(selectedRoles).ToArray<string>()); // seçili olanlar hariç rollerden çıkart dedik

                        return Redirect("/admin/user/list");

                    }

                }
                return Redirect("/admin/user/list"); // kullanıcı yoksada sayfaya gönder
            }
            return View(model);
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> RoleEdit(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            var members = new List<User>();
            var nonmembers = new List<User>();

            foreach (var user in _userManager.Users) // bütün kullanıcıları gezdik 
            {
                var list =  await _userManager.IsInRoleAsync(user,role.Name)?members:nonmembers; // kullanıcı o roldemi kontrol eder true  false deger döner 
                list.Add(user); // nonmembers yada members ekler   

            }

            var model = new RoleDetails()
            {
              Members =members,
              NonMembers = nonmembers,
              Role = role
            };
            return View(model);
        } 

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if(ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[]{}) // dizi null gelmişse boş dizi gönder
                {
                    var user = await _userManager.FindByIdAsync(userId);

                    if(user!=null)
                    {
                       var result = await _userManager.AddToRoleAsync(user,model.RoleName);
                       if(!result.Succeeded)
                       {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description); 
                            } 
                       }
                    }
                }


                foreach (var userId in model.IdsToRemove ?? new string[]{}) // dizi null gelmişse boş dizi gönder 
                {
                    var user = await _userManager.FindByIdAsync(userId);

                    if(user!=null)
                    {
                       var result = await _userManager.RemoveFromRoleAsync(user,model.RoleName);
                       if(!result.Succeeded)
                       {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description); 
                            } 
                       }
                    }
                }
            }
            return Redirect("/admin/role/"+model.RoleId); 
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles); // role list sayfasına rollerin bulundugu dizi yolladık
        }
        [Authorize(Roles="Admin")]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name)); // rol ekledik
                
                if(result.Succeeded)
                {
                    return RedirectToAction("RoleList"); // ekleme başarılı ise rolelist yönlendirdik
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult ProductList()
        {

            return View(new ProductListViewModel()
            {    
                Products = _productService.GetAll()
            });
        }


        public IActionResult CategoryList()
        {

            return View(new CategoryListViewModel()
            {    
                Categories = _categoryService.GetAll()
            });
        }
        
        [HttpGet]
        
        public IActionResult ProductCreate(){
           
           return View(); // CreateProduct view getirir (o sayfaya yönlenmemizi sağlar)
        }

        
        [HttpPost]
        public IActionResult ProductCreate(ProductModel model){

            if(ModelState.IsValid) // ProductModel ekledimiz zorunlulukları yerine getiriyormu
            {
                var entity = new Product()
             {
                Name = model.Name,
                Url = model.Url,
                Price = model.Price,
                Description = model.Description,
                ImageUrl = model.ImageUrl
             };            
             
              if(_productService.Create(entity)) // busines sınıfında bir hataya takılmazsa 
              {       
                      TempData.Put("message",new AlertMessage(){

                           Title = "İstek Eklendi",
                           Message ="Admin kontrolünden sonra eklenecek",
                           AlertType ="success"         
                            });
                      
                      return Redirect("/Home/Index"); // işlem bittiginde bu metoda yollanacak (View() dersek tekrar aynı sayfada oluruz)
              }
              
              TempData.Put("message",new AlertMessage(){

                           Title = "HAta",
                           Message =_productService.ErrorMessage,
                           AlertType ="danger"         
                            });
              
              return View(model);
             
            }
            

            return View(model); // girdigi bilgiler hatalı ise aynı sayfaya hatalı bilgilerle kalır

            
             
        }



        [HttpGet]
        public IActionResult CategoryCreate(){
           
           return View(); // CreateProduct view getirir (o sayfaya yönlenmemizi sağlar)
        }

        
        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model){
          
          if(ModelState.IsValid)
          {
              var entity = new Category()
             {
                Name = model.Name,
                Url = model.Url
                
             };            
             
              _categoryService.Create(entity);

             var msg = new AlertMessage()
           {
               Message = $"{entity.Name} isimli category eklendi",

               AlertType = "success"
           };
             
               
              TempData["message"] = JsonConvert.SerializeObject(msg); // serilize edilen obje (json objesine dönüştürdük)

             return RedirectToAction("CategoryList"); // işlem bittiginde bu metoda yollanacak (View() dersek tekrar aynı sayfada oluruz)
          }

          return View(model);

            
             
        }

        
        [HttpGet]
        public IActionResult ProductEdit(int? id){
           
           if(id ==null){
               return NotFound();
           }

           var entity = _productService.GetByIdWithCategories((int)id);

           if(entity == null){
               return NotFound();
           }

           var model = new ProductModel()
           {
             ProductId = entity.ProductId,
             Price = entity.Price,
             Name = entity.Name,
             Description = entity.Description,
             Url = entity.Url,
             ImageUrl = entity.ImageUrl,
             IsApproved = entity.IsApproved,
             IsHome = entity.IsHome,
             SelectedCategories = entity.ProductCategories.Select(i=>i.Category).ToList()
           };

           ViewBag.Categories = _categoryService.GetAll();

           return View(model); // tıkladımızda edit sayfasına gitmek için httpget oldu ve aldımız bilgiyi sayfaya taşıyoruz
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds , IFormFile file){ //  düzenleme sayfasında post edildiginde gelecegi yer
           
          
          if(ModelState.IsValid)
          {
              var entity = _productService.GetById(model.ProductId);

           if(entity == null){
               return NotFound();
           } 
           // güncelleme 

           entity.Name = model.Name;
           entity.Price = model.Price;
           entity.Url = model.Url;
           entity.Description = model.Description;
           entity.IsApproved = model.IsApproved;
           entity.IsHome = model.IsHome;

           if(file!=null)
           {
               
               var extention = Path.GetExtension(file.FileName); // uzantısını getiren fonk (.jpg)
               var randomName = string.Format($"{Guid.NewGuid()}{extention}"); // birbiri ile aynı olmayan isim halinde kaydetmeliyiz
               entity.ImageUrl = randomName; 
               var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",randomName);  // images dosya uzantısını aldık ve oraya kaydedeceğimiz isimi söyledik

               using(var stream = new FileStream(path,FileMode.Create)) // ilgili path e kopyalama 
               {
                   await file.CopyToAsync(stream);  // methodu async tanımlamalıyız
               } 
           }
            
           if(_productService.Update(entity,categoryIds))
           {    
                TempData.Put("message",new AlertMessage(){

                           Title = "ürün güncellendi",
                           Message ="ürün güncellendi",
                           AlertType ="success"         
                            });
                
                return RedirectToAction("ProductList");
           }
                TempData.Put("message",new AlertMessage(){

                           Title = "Hata",
                           Message =_productService.ErrorMessage,
                           AlertType ="danger"         
                            });
                
                           
          }

          ViewBag.Categories = _categoryService.GetAll(); // post işlemi bu bilgiyi göndermezse hata alırız  
          return View(model);
           
        }




         [HttpGet]
        public IActionResult CategoryEdit(int? id){
           
           if(id ==null){
               return NotFound();
           }

           var entity = _categoryService.GetByIdWithProducts((int)id);

           if(entity == null){
               return NotFound();
           }

           var model = new CategoryModel()
           {
             CategoryId = entity.CategoryId,
            
             Name = entity.Name,
            
             Url = entity.Url,

             Products = entity.ProductCategories.Select(p=>p.Product).ToList() // başka bir tablodan getirdimiz  productlarıda aldık 
             
           };

           return View(model); // tıkladımızda edit sayfasına gitmek için httpget oldu ve aldımız bilgiyi sayfaya taşıyoruz
        }

        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model){ //  düzenleme sayfasında post edildiginde gelecegi yer
           

           if(ModelState.IsValid)
           {
              var entity = _categoryService.GetById(model.CategoryId);

           if(entity == null){
               return NotFound();
           } 
           // güncelleme 

           entity.Name = model.Name;
          
           entity.Url = model.Url;
           
           _categoryService.Update(entity);

            var msg = new AlertMessage()
           {
               Message = $"{entity.Name} isimli category güncellendi",

               AlertType = "success"
           };
             
               
              TempData["message"] = JsonConvert.SerializeObject(msg);

           
           
           return RedirectToAction("CategoryList"); // işlemler bittikten sonra ProductList action a git

           }
         
          return View(model); 
           
        }

       
       public IActionResult DeleteProduct(int ProductId)
       {
           
           var entity = _productService.GetById(ProductId);

           if(entity ==null){
               return NotFound();
           }

           _productService.Delete(entity);

            var msg = new AlertMessage()
           {
               Message = $"{entity.Name} isimli ürün silindi",

               AlertType = "danger"
           };
             
               
              TempData["message"] = JsonConvert.SerializeObject(msg);
          
            
           return RedirectToAction("ProductList");
       }



        public IActionResult DeleteCategory(int categoryId)
        {
           
           var entity = _categoryService.GetById(categoryId);

           if(entity ==null){
               return NotFound();
           }

           _categoryService.Delete(entity);

            var msg = new AlertMessage()
           {
               Message = $"{entity.Name} isimli category silindi",

               AlertType = "danger"
           };
             
               
              TempData["message"] = JsonConvert.SerializeObject(msg);
          
            
           return RedirectToAction("CategoryList");
       }

       [HttpPost]
       public IActionResult DeleteFromCategory(int productId , int categoryId)
       {
           
           _categoryService.DeleteFromCategory(productId,categoryId);

           return Redirect("/admin/categories/"+categoryId);
       }




    }
}