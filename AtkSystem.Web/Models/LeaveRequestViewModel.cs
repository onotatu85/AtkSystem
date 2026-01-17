using System.ComponentModel.DataAnnotations;
using AtkSystem.Core.Enums;

namespace AtkSystem.Web.Models;

public class LeaveRequestViewModel
{
    [Required(ErrorMessage = "休暇の種類を選択してください")]
    [Display(Name = "休暇種類")]
    public LeaveType LeaveType { get; set; }

    [Required(ErrorMessage = "開始日は必須です")]
    [DataType(DataType.Date)]
    [Display(Name = "開始日")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "終了日は必須です")]
    [DataType(DataType.Date)]
    [Display(Name = "終了日")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "申請理由は必須です")]
    [MaxLength(500, ErrorMessage = "理由は500文字以内で入力してください")]
    [Display(Name = "申請理由")]
    public string Reason { get; set; } = string.Empty;
}
