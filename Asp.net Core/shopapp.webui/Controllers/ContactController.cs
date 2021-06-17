using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.webui.Identity;

namespace shopapp.webui.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private  IContactService _contactService;
        private  UserManager<User> _userManager;
     
        public ContactController(IContactService contactService,UserManager<User> userManager)
        {
            _contactService = contactService;
            _userManager = userManager;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] 
        public IActionResult Create(string Message,string mail)
        {
            var UserId = _userManager.GetUserId(User);
            // Console.WriteLine(UserId);
            // Console.WriteLine(Message);
            
            _contactService.CreateMessage(UserId,Message,mail);

            return RedirectToAction("Index","Home");
        }
        
        [Authorize(Roles="Admin")]
        public IActionResult GetAll()
        {
            var list = _contactService.GetAll();
            return View(list);
        }  

    }
}