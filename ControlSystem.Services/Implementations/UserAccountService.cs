using ControlSystem.Domain.Response;
using ControlSystem.Domain.ViewModels;
using ControlSystem.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ControlSystem.Services.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        private readonly ILogger<UserAccountService> _logger;
        public Task<ErrorResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorResponse<ClaimsIdentity>> Register(RegisterViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
