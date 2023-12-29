using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UserIdentityApp.Models
{
    public static class IdentitySeedData{     //Başlangıç verilerini bu metod içinde bildir.
        
        private const string adminUser = "admin";
        private const string adminPassword = "Admin_123";

        public static async void IdentityTestUser(IApplicationBuilder app){
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();

            if(context.Database.GetAppliedMigrations().Any()){        // içerisinde veri var mı yok mu kontrol et ! ona göre işlemler devam etsin. Yoksa yukarıda tanımladığım admin değerlerimi ata.
                context.Database.Migrate();
            }
            var UserManager = app.ApplicationServices.CreateScope().ServiceProvider. GetRequiredService<UserManager<AppUser>>();                     // kontrol sağlaması yapıldı.

            var user = await UserManager.FindByIdAsync(adminUser);
             if(user == null){              // herhangi bir user bilgisi yoksa
                user = new AppUser{
                    FullName="Ahmet Kaya",
                    Email = "admin@ahmetkaya.com",
                    PhoneNumber = "12345678912"
                };
            await UserManager.CreateAsync(user,adminPassword);
        }
    }
}
}