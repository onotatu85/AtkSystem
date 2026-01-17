using System.ComponentModel.DataAnnotations;

namespace AtkSystem.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "社員IDは必須です")]
    [Display(Name = "社員ID")]
    public string EmployeeId { get; set; } = string.Empty;

    [Required(ErrorMessage = "パスワードは必須です")]
    [DataType(DataType.Password)]
    [Display(Name = "パスワード")]
    public string Password { get; set; } = string.Empty;
}
