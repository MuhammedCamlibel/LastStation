using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.webui.EmailServices;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [AutoValidateAntiforgeryToken] // bütün post metotları için kontrol eder 
   // [Authorize]
    public class AccountController:Controller
    {   

        private UserManager<User> _usermanager; // temel kullanıcı  kayıtları yapabilmek için ıdentity nesnesi 

        private SignInManager<User> _signInManager; // cookie bilgilerini yönetmek için 

        private IEmailSender _emailsender;
        private ICartService _cartService;

        public AccountController(UserManager<User> usermanager,SignInManager<User> signInManager,IEmailSender emailsender,ICartService cartService )
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
            _emailsender = emailsender;
            _cartService = cartService; 
        }

        [HttpGet]  
       // [AllowAnonymous] // login olmasa bile görebilir (herkez)
        public IActionResult Login(string ReturnUrl=null)
        {
           return View(new LoginModel(){
               ReturnUrl = ReturnUrl
           });
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            } 

            //var user = await _usermanager.FindByNameAsync(model.UserName); // kullanıcı adı sorguladık 
            var user = await _usermanager.FindByEmailAsync(model.Email); // email sorguladık 

            if(user == null)
            {
               ModelState.AddModelError("","Böyle bir kullanıcı bulunamadı");
               return View(model);              
            }

            if(!await _usermanager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("","Mailinizi onaylamalısınız");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user,model.Password,true,false); // giriş işlemi 

            if(result.Succeeded)
            {  
                return Redirect(model.ReturnUrl??"~/"); // eger nulsa anasayfaya git dedik
            }
            
            
            ModelState.AddModelError("","Kullanıcı Adı veya Parola Yanlış");
            
            return View(model);
           
        } 

        public IActionResult Register()
        {
          return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // servise form üstünden token atar onu kontrol eder

        public async  Task<IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
               FirstName = model.FirstName,
               LastName = model.LastName,
               UserName = model.UserName,
               Email = model.Email

            };

            var result = await _usermanager.CreateAsync(user,model.Password); // şifreyi hashli şekilde kayıt ettik

            if(result.Succeeded)
            {
                // generate token 
                var code = await _usermanager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail","Account",new{
                     userId = user.Id,
                     token = code  
                });
                
                // email 

                await _emailsender.SendEmailAsync(model.Email,"Hesabınızı Onaylayın",$"Lütfen email hesabınızı onaylamak için linke <a href='https://localhost:5001{url}'>tıklayınız.</a>");
                return RedirectToAction("Login","Account");
            }

            ModelState.AddModelError("","Bilinmeyen bir hata oluştu lütfen tekrar deneyin"); // Modelde hata oluşmayıp server da oluşursa (startup a yazdımız gereksinimler karşılanmazsa)
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Put("message",new AlertMessage(){

                           Title = "Oturum Kapatıldı",
                           Message ="Hesabınız Güvenli Bir Şekilde Kapatıldı",
                           AlertType ="success"         
                            });

            return Redirect("~/");
        } 

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {    

            if(userId ==null || token == null)
            { 
                TempData.Put("message",new AlertMessage(){

                    Title = "Geçersiz Token",
                    Message ="Geçersiz Token",
                    AlertType ="danger"         
                });
                 
                return View();
            }

            var user = await _usermanager.FindByIdAsync(userId);

            if(user != null)
            {

                   var result = await _usermanager.ConfirmEmailAsync(user,token);

                    if(result.Succeeded)
                    {   
                        // card objesini oluşturuyoruz 
                        _cartService.InitializeCart(user.Id);

                        TempData.Put("message",new AlertMessage(){

                           Title = "Hesabınız Onaylandı",
                           Message ="Hesabınız Onaylandı",
                           AlertType ="success"         
                            });
                        
                        return View();
                    }
  
            }
                TempData.Put("message",new AlertMessage(){

                           Title = "Hesabınız Onaylanmadı",
                           Message ="Hesabınız Onaylanmadı",
                           AlertType ="warning"         
                            });
                
                return View();

           
        }

        
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
           if(string.IsNullOrEmpty(Email))
           {
               return View();
           }

           var user = await _usermanager.FindByEmailAsync(Email);
           if(user==null)
           {
               return View();
           }

           var code = await _usermanager.GeneratePasswordResetTokenAsync(user);

           // generate token 
                
                var url = Url.Action("ResetPassword","Account",new{
                     userId = user.Id,
                     token = code  
                });
                
                // email 

                await _emailsender.SendEmailAsync(Email,"Reset Password",$"Parolanızı yenilemek için linke <a href='https://localhost:5001{url}'>tıklayınız.</a>");
           return View();
        }
        public IActionResult ResetPassword(string userId , string token)
        {
            
            if(userId ==null || token==null)
            {
              return RedirectToAction("Home","Index");
            }
            var model = new ResetPasswordModel()
            {
              Token = token
            };
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if(!ModelState.IsValid)
            {
               return View(model);
            }  

            var user = await _usermanager.FindByEmailAsync(model.Email);
            if(user==null)
            {
                return RedirectToAction("Home","Index");
            }

            var result = await _usermanager.ResetPasswordAsync(user,model.Token,model.Password);

            if(result.Succeeded)
            {
                return RedirectToAction("Login","Account");
            }
            return View(model);
        }


        public IActionResult AccessDenied()
        {

            return View();
        }
        
    }
}