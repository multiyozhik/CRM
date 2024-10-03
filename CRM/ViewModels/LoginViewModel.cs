using System.ComponentModel.DataAnnotations;

namespace CRMSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Недопустимое имя")]
        public string? UserName { get; set; }

        //[DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Некорректный email")]
        public string? Email { get; set; }

        //[DataType(DataType.Password)]
        [Required(ErrorMessage = "Некорректный пароль")]
        public string? Password { get; set; }
    }
}
