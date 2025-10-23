using RentMaster.Core.Auth.Types;

namespace RentMaster.Core.Auth.Interface;

public interface IAuthService
{
    Task<string?> LoginAsync(string gmail, string password, UserTypes type);
}