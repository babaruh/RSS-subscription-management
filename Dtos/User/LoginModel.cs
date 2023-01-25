using System.ComponentModel.DataAnnotations;

namespace TestTask.Dtos.User;

public class LoginModel
{
    [Required] 
    public string Username { get; set; }
    
    [Required] 
    public string Password { get; set; }
}