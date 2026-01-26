namespace Daira.Application.UseCases
{
    public class AuthUseCase(ITokenGenerator tokenGenerator, IUserRepository userRepository)
    {
        public async Task<RegisterUserResponseDto> RegisterUser(RegisterUserDto registerUserDto)
        {
            if (await userRepository.FindByEmailAsync(registerUserDto.Email)) throw new Exception("Email already in use.");
            if (await userRepository.FindByUserNameAsync(registerUserDto.UserName)) throw new Exception("User Name is Already  in use");

            var user = new AppUser
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
                PhoneNumber = registerUserDto.PhoneNumber,
                DisplayName = registerUserDto.DisplayName,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Bio = registerUserDto.Bio,

            };

            var result = await userRepository.CreateUser(user, registerUserDto.Password);
            var token = await tokenGenerator.GenerateJwtToken(user);
            var userDto = new RegisterUserResponseDto
            {
                Message = "UserCreated",
                Success = true,
                Token = token,

            };
            return userDto;

        }
    }
}