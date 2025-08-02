using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using QuizApi.Entities;
using QuizApi.Interfaces;
using QuizApi.Models;
using QuizApi.Persistence;

namespace QuizApi.Controllers;

public class AccountController: BaseApiController
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenService  _tokenService;

    public AccountController(IAccountRepository accountRepository, ITokenService tokenService)
    {
        _accountRepository = accountRepository;
        _tokenService = tokenService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> RegisterAccount(RegisterDto dto)
    {
        var registeredUser = await _accountRepository.RegisterUser(dto);

        return registeredUser;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginAccount(LoginDto dto)
    {
        try
        {
            var userWithoutToken = await _accountRepository.LoginUser(dto);
            return new UserDto
            {
                UserName = userWithoutToken.UserName,
                DisplayName = userWithoutToken.DisplayName,
                ImageUrl = "",
                Token = _tokenService.CreateToken(userWithoutToken)
            };
        }
        catch (Exception e)
        {
            return Unauthorized("Kombination stimmt nicht.");
        }
    }

}