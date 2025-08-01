using QuizApi.Entities;
using QuizApi.Models;

namespace QuizApi.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}