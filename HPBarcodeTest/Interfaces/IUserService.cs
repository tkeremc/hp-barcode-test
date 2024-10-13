using HPBarcodeTest.Models;

namespace HPBarcodeTest.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> Register(string email, string password);
        Task<UserModel> Login(string email, string password);
        Task<UserModel> GetUserById(string id);
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel> GetUserByEmail(string email);  // Bu metodu ekledik
        Task<UserModel> UpdateUser(string id, string email, string password);
        Task<bool> DeleteUser(string id);
    }
}