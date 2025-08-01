using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizApi.Entities;
using QuizApi.Models;

namespace QuizApi.Persistence;

public class AccountRepository: IAccountRepository
{
    private readonly IMongoCollection<AppUser> _usersCollection;

    public AccountRepository(IOptions<AccountDatabaseSettings> userDatabaseSettings)
    {
        var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(userDatabaseSettings.Value.DatabaseName);
        _usersCollection = mongoDatabase.GetCollection<AppUser>(userDatabaseSettings.Value.AccountsCollectionName);
    }

    public async Task<List<AppUser>> GetAllUsers() 
        => await _usersCollection.Find(u => true).ToListAsync();

    public async Task<AppUser> RegisterUser(RegisterDto dto)
    {
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = dto.Username,
            DisplayName = dto.DisplayName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            PasswordSalt = hmac.Key
        };

        _usersCollection.InsertOne(user);
        
        return user;
    }

    public async Task<AppUser> LoginUser(LoginDto dto)
    {
        var filter = Builders<AppUser>.Filter.Eq(user => user.UserName, dto.Username);
        var matchingUsers = await _usersCollection.Find(filter).ToListAsync();
        
        var matchingUser = matchingUsers.Single();

        using var hmac = new HMACSHA512(matchingUser.PasswordSalt);
        
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != matchingUser.PasswordHash[i]) 
                throw new ArgumentException("Passwords do not match.");
            
        }

        return matchingUser;
    }
}