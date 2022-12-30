namespace Net7Practice.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(Users users, string Password);
        Task<ServiceResponse<string>> Login(string UserName, string password);
        Task<bool> UserExists(string UserName);
    }
}
