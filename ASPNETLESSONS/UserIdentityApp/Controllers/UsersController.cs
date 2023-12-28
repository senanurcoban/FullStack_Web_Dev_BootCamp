using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserIdentityApp.ViewModels;
using UserIdentityApp.Models;


namespace UserIdentityApp.Controllers
{
     public class UsersController : Controller{

        private UserManager<IdentityUser> _userManager;

         public UsersController(UserManager<IdentityUser> userManager){
             _userManager = userManager;
         }
         public IActionResult Index (){
            return View(_userManager.Users);
         }

        public IActionResult Create (){
            return View();
         }

        [HttpPost]
        public async Task<IActionResult> Create (CreateViewModel model){   
        // async mantığı: yapılan işlem devam ederken arada olması gereken işlemi yapıp tekrar yapılması gereken işe devam ediyor.

          if(ModelState.IsValid){
            var user=new IdentityUser{UserName=model.UserName,Email=model.Email};
            IdentityResult result =await _userManager.CreateAsync(user,model.Password);

            if(result.Succeeded){
                return RedirectToAction("Index");
            }
            foreach(IdentityError err in result.Errors){
                ModelState.AddModelError("",err.Description);
            }
             
          }

            return View();
         }
          


     }


     
}