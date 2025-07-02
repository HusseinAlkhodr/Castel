using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Castel.Models.Authentication;

namespace Castel.DTO
{
    public class AccountDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public AccountStatus Status { get; set; }
        public AccountType AccountType { get; set; }
    }
    public class RegisterDTO
    {
        [Required(ErrorMessage = "الاسم  مطلوب")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "الكنية مطلوبة")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد غير صحيحة")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقتين")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginDTO
    {
        [Required(ErrorMessage = "البريد الالكتروني  مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد غير صحيحة")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
