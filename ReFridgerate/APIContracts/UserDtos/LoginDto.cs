namespace APIContracts.UserDtos;

public class LoginDto
{
    public required string email { get; set; }
    public required string password { get; set; }

    public LoginDto(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}