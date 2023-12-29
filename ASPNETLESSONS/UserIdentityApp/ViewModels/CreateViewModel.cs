using System.ComponentModel.DataAnnotations;

namespace UserIdentityApp.ViewModels
{
    public class CreateViewModel{                 
     // Yeni eklemek istenilen kullancının kayıt aşamasında hangi bilgilerini almalıyız?

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]                                          // zorunlu alan olarak belirtme
        [DataType(DataType.Password)]                      // şifrenin gözükmemesi için */.
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Parolalar eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }



    
}



