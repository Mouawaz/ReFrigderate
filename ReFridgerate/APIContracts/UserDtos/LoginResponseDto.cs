namespace APIContracts.UserDtos;

public class LoginResponseDto
{
    public bool success { get; set; }
    public int userId { get; set; }
    public string fullName { get; set; }
    public string role { get; set; }
}