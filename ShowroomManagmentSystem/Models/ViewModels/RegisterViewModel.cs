namespace ShowroomManagmentSystem.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
