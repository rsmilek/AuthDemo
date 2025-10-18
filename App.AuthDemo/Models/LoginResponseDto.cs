namespace App.AuthDemo.Models
{
    public class LoginResponseDto
    {
        public UserDto? User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}