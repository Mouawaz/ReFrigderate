namespace APIContracts.UserDtos;

public class UserDto
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string Role { get; set; }
}

public class UpdateUserRoleDto
{
    public int UserId { get; set; }
    public string NewRole { get; set; }
}