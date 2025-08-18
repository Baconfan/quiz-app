using QuizApi.Dtos.Input;
using QuizApi.Entities;
using QuizApi.Models;

namespace QuizApi.Persistence;

public interface IAccountRepository
{
    Task<List<AppUser>>  GetAllUsers();
    
    Task<AppUser> RegisterUser(RegisterDto dto);
    
    Task<AppUser> LoginUser(LoginDto dto);
}