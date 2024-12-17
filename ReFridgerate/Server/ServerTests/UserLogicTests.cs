using APIContracts.UserDtos;
using BusinessLayer;
using FakeItEasy;
using FluentAssertions;
using GrpcClient;

namespace ServerTests;

public class UserLogicTests
{
    private UserLogic userLogic;
    private IUserClientManager clientManager;

    public UserLogicTests()
    {
        clientManager = A.Fake<IUserClientManager>();
        userLogic = new UserLogic(clientManager);
    }

    [Fact]
    public void UserLogic_GetSingleAsync_ShouldReturnUser()
    {
        int id = 1;
        A.CallTo(() => clientManager.GetSingleAsync(id)).Returns(A.Fake<UserDto>());

        var result = userLogic.GetSingleAsync(id);
        
        result.Should().BeOfType<Task<UserDto>>();
    }
    [Fact]
    public async Task UserLogic_GetSingleAsync_ShouldReturnException()
    {
        int id = 0;
        Func<Task> act = async () => await userLogic.GetSingleAsync(id);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Provided user id is not valid");
    }
    [Fact]
    public void UserLogic_GetSingleAsync_ShouldReturnLoginResponseDto()
    {
        LoginDto loginDto = new LoginDto()
        {
            email = "someone@gmail.com",
            password = "ds1234531"
        };
        A.CallTo(() => clientManager.LoginAsync(loginDto)).Returns(A.Fake<LoginResponseDto>());

        var result = userLogic.LoginAsync(loginDto);
        
        result.Should().BeOfType<Task<LoginResponseDto>>();
    }
    [Theory]
    [InlineData(null,"sad123123")]
    [InlineData("someone@gmail.com", null)]
    public async Task UserLogic_LoginAsync_ShouldReturnFieldsEmpty(string email, string password)
    {
        LoginDto loginDto = new LoginDto()
        {
            email = email,
            password = password
        };
        Func<Task> act = async () => await userLogic.LoginAsync(loginDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Fields cannot be empty");
    }
    [Theory]
    //[InlineData("someone@")]
    [InlineData("someonegmail.com")]
    public async Task UserLogic_LoginAsync_ShouldReturnInvalidEmail(string email)
    {
        LoginDto loginDto = new LoginDto()
        {
            email = email,
            password = "password1234"
        };
        Func<Task> act = async () => await userLogic.LoginAsync(loginDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid email");
    }
    [Fact]
    public async Task UserLogic_LoginAsync_ShouldReturnPasswordShort()
    {
        LoginDto loginDto = new LoginDto()
        {
            email = "someone@gmail.com",
            password = "pass"
        };
        Func<Task> act = async () => await userLogic.LoginAsync(loginDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Password has to be longer than 5 characters");
    }
    
    [Fact]
    public async Task UserLogic_LoginAsync_ShouldReturnPasswordOnlyDigits()
    {
        LoginDto loginDto = new LoginDto()
        {
            email = "someone@gmail.com",
            password = "1234531"
        };
        Func<Task> act = async () => await userLogic.LoginAsync(loginDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Password cannot contain only digits");
    }
    [Fact]
    public void UserLogic_UpdateAsync_ShouldReturnBool()
    {
        int id = 1;
        int role = 1;
        A.CallTo(() => clientManager.UpdateUserAsync(id, role)).Returns(true);

        var result = userLogic.UpdateAsync(id, role);
        
        result.Should().BeOfType<Task<bool>>();
    }
    
    [Fact]

    public async Task UserLogic_UpdateAsync_ShouldReturnInformationInvalid()
    {
        int id = 0;
        int role = 1;
        Func<Task> act = async () => await userLogic.UpdateAsync(id, role);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Provided information is not valid");
    }
    [Fact]
    public void UserLogic_AddAsync_ShouldReturnLoginResponseDto()
    {
        CreateUserDto userDto = new CreateUserDto()
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "someone@gmail.com",
            Password = "ds1234531"
        };
        A.CallTo(() => clientManager.AddAsync(userDto)).Returns(A.Fake<LoginResponseDto>());

        var result = userLogic.AddAsync(userDto);
        
        result.Should().BeOfType<Task<LoginResponseDto>>();
    }
    [Theory]
    [InlineData(null, "Smith", "someone@gmail.com", "password123")]
    [InlineData("Smith", null, "someone@gmail.com", "password123")]
    [InlineData("Smith", "Smith", null, "password123")]
    [InlineData("Smith", "Smith", "someone@gmail.com", null)]
    public async Task UserLogic_AddAsync_ShouldReturnFieldsEmpty(string firstName, string lastName, string email, string password)
    {
        CreateUserDto userDto = new CreateUserDto()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };
        Func<Task> act = async () => await userLogic.AddAsync(userDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Fields cannot be empty");
    }
    [Theory]
    [InlineData("John1", "Smith")]
    [InlineData("John", "Smith1")]
    public async Task UserLogic_AddAsync_ShouldReturnCannotContainDigits(string firstName, string lastName)
    {
        CreateUserDto userDto = new CreateUserDto()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = "someone@gmail.com",
            Password = "ds1234531"
        };
        Func<Task> act = async () => await userLogic.AddAsync(userDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("First and Last name cannot contain digits");
    }
    [Fact]
    public async Task UserLogic_AddAsync_ShouldReturnPasswordShort()
    {
        CreateUserDto userDto = new CreateUserDto()
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "someone@gmail.com",
            Password = "ds12"
        };
        Func<Task> act = async () => await userLogic.AddAsync(userDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Password has to be longer than 5 characters");
    }
}