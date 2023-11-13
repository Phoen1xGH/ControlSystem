using ControlSystem.Domain.Response;
using ControlSystem.Domain.ViewModels;
using System.Security.Claims;

namespace ControlSystem.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<ErrorResponse<ClaimsIdentity>> Register(RegisterViewModel model);

        Task<ErrorResponse<ClaimsIdentity>> Login(LoginViewModel model);
    }
}
