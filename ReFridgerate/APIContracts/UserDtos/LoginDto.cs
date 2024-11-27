namespace APIContracts.UserDtos;

public class LoginDto
{
    public string email { get; set; }
    public string password { get; set; }

    public LoginDto(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}