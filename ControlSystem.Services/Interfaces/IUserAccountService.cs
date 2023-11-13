using ControlSystem.Domain.Response;
using ControlSystem.Domain.ViewModels;
using System.Security.Claims;

namespace ControlSystem.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
    }
}
