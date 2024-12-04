namespace ShowroomManagmentSystem.Models.ViewModels
{
    public class ForgotPasswordModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }
    }
}
