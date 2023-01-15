using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models
{
    public class ConfirmResetPasswordForm
    {
        public string CodeBase64Url { get; set; }

        [Display(Name = "Nova Password")]
        public string newPassword { get; set; }

        [Display(Name = "Reescreva a nova password")]
        public string newPasswordConfirmation { get; set; }
    }
}
