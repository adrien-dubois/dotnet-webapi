namespace WebApi.Models.Users;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class CreateRequest
{
    private int role = 1;

    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Lastname { get; set; }

    [EnumDataType(typeof(Role))]
    public int Role { 
        get => role; 
        set => role = value; 
        }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}