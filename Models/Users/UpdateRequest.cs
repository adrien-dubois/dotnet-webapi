namespace WebApi.Models.Users;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class UpdateRequest
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }

    [EnumDataType(typeof(Role))]
    public string Role { get; set; }
}