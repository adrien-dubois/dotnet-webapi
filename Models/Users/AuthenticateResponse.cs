namespace WebApi.Models.Users;

public class AuthenticateResponse
{
    public int id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
}