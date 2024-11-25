namespace APIContracts.UserDtos;

public class UserDto
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string DateOfBirth { get; set; }
    public required string Sex { get; set; }
    public required string PhoneNumber { get; set; }
}