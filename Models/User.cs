using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestTask.Models;

public class User
{
    public User()
    {
        Id = Guid.NewGuid();
    }
    
    [Key]
    public Guid Id { get; }
    public string Username { get; set; }
    
    [JsonIgnore]
    public string Password { get; set; }

}