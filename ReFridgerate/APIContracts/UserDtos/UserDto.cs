namespace APIContracts.UserDtos;

public class UserDto
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public int Role { get; set; }
}
