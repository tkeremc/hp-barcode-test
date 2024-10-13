using HPBarcodeTest.DbContext;
using HPBarcodeTest.Interfaces;
using HPBarcodeTest.Models;
using HPBarcodeTest.Utils;
using MongoDB.Driver;

namespace HPBarcodeTest.Services;

public class UserService : IUserService
{
    private readonly IMongoCollection<UserModel> _userCollection;
        
            public UserService(IMongoDbContext mongoDbContext)
            {
                _userCollection = mongoDbContext.GetCollection<UserModel>(AppSettingConfig.Configuration["MongoDBSettings:UserCollection"]!);
            }
    
            public async Task<UserModel> Register(string email, string password)
            {
                var user = new UserModel()
                {
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
                };
                await _userCollection.InsertOneAsync(user);
                return user;
            }
    
            public async Task<UserModel> Login(string email, string password)
            {
                var user = await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    return null;
                return user;
            }
    
            public async Task<UserModel> GetUserById(string id)
            {
                return await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            }
            
            public async Task<UserModel> GetUserByEmail(string email)
            {
                return await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
            }
    
            public async Task<List<UserModel>> GetAllUsers()
            {
                return await _userCollection.Find(user => true).ToListAsync();
            }
    
            public async Task<UserModel> UpdateUser(string id, string email, string password)
            {
                var user = await GetUserById(id);
                if (user == null)
                    return null;
    
                user.Email = email ?? user.Email;
                if (!string.IsNullOrWhiteSpace(password))
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    
                await _userCollection.ReplaceOneAsync(u => u.Id == id, user);
                return user;
            }
    
            public async Task<bool> DeleteUser(string id)
            {
                var result = await _userCollection.DeleteOneAsync(user => user.Id == id);
                return result.DeletedCount > 0;
            }
}