using System.ComponentModel.DataAnnotations;

namespace AuthService.DTO;

public class SignUpDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } // User's email address (required for Identity)

    [Required]
    [MinLength(6)]
    public string Password { get; set; } // Password for the user

    [Required]
    public string Role { get; set; } // Role to assign (e.g., "User", "Admin")
}
