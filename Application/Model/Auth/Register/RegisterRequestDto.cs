using System.ComponentModel.DataAnnotations;

namespace Application.Model.Auth.Register
{
    public class RegisterRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;
    }
}