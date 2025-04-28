using System.ComponentModel.DataAnnotations;

namespace ExcelDownloadWithSignalR.Web.Models.ViewModels
{
    public record SignInViewModel([Required]string Email, [Required]string Password)
    {
    }
}
