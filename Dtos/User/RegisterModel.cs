using System.ComponentModel.DataAnnotations;

namespace TestTask.Dtos.User;

public class RegisterModel
{
    [Required] 
    public string Username { get; set; }
    
    [Required] 
    public string Password { get; set; }
}