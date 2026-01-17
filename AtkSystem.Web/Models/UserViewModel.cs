using System.ComponentModel.DataAnnotations;
using AtkSystem.Core.Entities;
using AtkSystem.Core.Enums;

namespace AtkSystem.Web.Models;

public class UserViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "社員IDは必須です")]
    [MaxLength(50)]
    [Display(Name = "社員ID")]
    public string EmployeeId { get; set; } = string.Empty;

    [Required(ErrorMessage = "氏名は必須です")]
    [MaxLength(100)]
    [Display(Name = "氏名")]
    public string FullName { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "パスワード")]
    public string? Password { get; set; }

    [Display(Name = "権限")]
    public Role Role { get; set; }

    [Display(Name = "所属")]
    public int? DepartmentId { get; set; }

    [Display(Name = "アカウント有効")]
    public bool IsActive { get; set; } = true;
    
    // For Display
    public string? DepartmentName { get; set; }
}
