using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserIdentityApp.Models;
using UserIdentityApp.ViewModels;

namespace UserIdentityApp.Controllers
{
    public class AccountController:Controller{

        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private IEmailSender _emailSender;
         public AccountController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IEmailSender emailSender){
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public IActionResult Login(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model){
             var user = await _userManager.FindByEmailAsync(model.Email);
             if(user != null){
                await _signInManager.SignOutAsync();

                 if(!await _userManager.IsEmailConfirmedAsync(user)){
                    ModelState.AddModelError("","Hesabınızı onaylayınız.");
                    return View(model);
                }

            var result = await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,true);
            if(result.Succeeded){
                    await _userManager.ResetAccessFailedCountAsync(user);
                    await _userManager.SetLockoutEndDateAsync(user,null);

                    return RedirectToAction ("Index","Home");
                }
                 else if(result.IsLockedOut){
                    var lockouteDate = await _userManager.GetLockoutEndDateAsync(user);
                    var timeLeft = lockouteDate.Value - DateTime.UtcNow;
                    ModelState.AddModelError("", $"Hesabınız kilitlendi, {timeLeft.Minutes} dakika sonra tekrar deneyiniz!");
                 }else{
                ModelState.AddModelError("","Hatalı Email ya da Parola");
                 }
             }else{
                 ModelState.AddModelError("","Bu email adresiyle bir hesap bulunamadı");
             }
            return View(model);
        }

         public IActionResult Create (){

            return View();
         }

        [HttpPost]
        public async Task<IActionResult> Create (CreateViewModel model){
           if(ModelState.IsValid){
                var user = new AppUser{UserName = model.UserName, Email = model.Email, FullName = model.FullName};
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if(result.Succeeded){
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail","Account",new {user.Id,token});

                    //email gönderme işlemi
                    await _emailSender.SendEmailAsync(user.Email, "Hesap Onayı",$"Lütfen mail hesabınızı onaylamak için linke <a href='http://localhost:5251{url}'> tıklayınız </a>.");
                    TempData["message"] = "Email hesabınızdaki onay mailine tıklayınız.";

                    return RedirectToAction("Login","Account");
                }
                foreach(IdentityError err in result.Errors){
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string Id, string token){

            if(Id == null || token == null){
                TempData["message"] = "Geçersiz token bilgisi";
                return View();
            }

            var user = await _userManager.FindByIdAsync(Id);
            if(user != null){
                var result = await _userManager.ConfirmEmailAsync(user,token);
                if(result.Succeeded){
                    TempData["message"] = "Hesabınız onaylandı";
                    return RedirectToAction("Login","Account");
                }
            }
            TempData["message"] = "Kullanıcı bulunamadı.";
            return View();
        }
         public async Task<IActionResult>Logout(){
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
         public IActionResult ForgotPassword(){
            return View();
        }

         [HttpPost]
         public async Task<IActionResult> ForgotPassword(string Email){

            if(string.IsNullOrEmpty(Email)){
                TempData["message"] = "Eposta adresinizi giriniz.";
                return View();
            }
            
            var user = await _userManager.FindByEmailAsync(Email);

            if(user == null){
                TempData["message"] = "Eposta adresiyle eşleşen kayıt bulunamadı.";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var url = Url.Action("ResetPassword","Account", new{user.Id, token});

            await _emailSender.SendEmailAsync(Email,"Parolamı Sıfırla", $"Parolanızı yenilemek için linke <a href='http://localhost:5251{url}'>tıklayınız</a>");

            TempData["message"] = "Eposta adresinize gönderilen link ile şifrenizi sıfırlayabilirsiniz.";

            return View();

        }

         public IActionResult ResetPassword(string Id, string token){
            if(Id == null || token == null){
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordModel{Token = token};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model){
            
            if(ModelState.IsValid){
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user == null){
                    TempData["message"] = "bu mail adresiyle eşleşen kullanıcı yok.";
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ResetPasswordAsync(user,model.Token,model.Password);
                if(result.Succeeded){
                    TempData["message"] = "Şifreniz başarılı bir şekilde değiştirildi";
                    return RedirectToAction("Login");
                }

                foreach(IdentityError err in result.Errors){
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View(model);
        }
    }
}