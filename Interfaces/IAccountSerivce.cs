using dotnet_Api.Entities;

namespace dotnet_Api.Interfaces
{
    public interface IAccountSerivce
    {
        Task Register(Account account);
        Task<Account> Login(string username, string password);
    }
}