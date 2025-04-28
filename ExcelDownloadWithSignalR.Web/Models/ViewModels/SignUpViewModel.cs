using System.ComponentModel.DataAnnotations;

namespace ExcelDownloadWithSignalR.Web.Models.ViewModels
{
    public record SignUpViewModel([Required]string Email, [Required]string Password, [Required]string ConfirmPassword);
}
