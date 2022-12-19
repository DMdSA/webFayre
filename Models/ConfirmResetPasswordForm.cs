namespace WebFayre.Models
{
    public class ConfirmResetPasswordForm
    {
        public string CodeBase64Url { get; set; }
        public string newPassword { get; set; }
        public string newPasswordConfirmation { get; set; }
    }
}
