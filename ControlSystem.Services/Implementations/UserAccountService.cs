using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Helpers;
using ControlSystem.Domain.Response;
using ControlSystem.Domain.ViewModels;
using ControlSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ControlSystem.Services.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        private readonly ILogger<UserAccountService> _logger;

        private readonly IRepository<UserAccount> _repository;

        public UserAccountService(ILogger<UserAccountService> logger, IRepository<UserAccount> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _repository.GetAll().FirstOrDefaultAsync(x => x.Username == model.Name);

                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = StatusCode.UserNotFound.GetDescriptionValue()
                    };
                }

                if (user.Password != HashPasswordHelper.HashPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Неверный логин или пароль"
                    };
                }

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>
                {
                    StatusCode = StatusCode.OK,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Login]: {ex.Message}");

                return new BaseResponse<ClaimsIdentity>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
        {
            try
            {
                var user = await _repository.GetAll().FirstOrDefaultAsync(x => x.Username == model.Name);

                if (user != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        StatusCode = StatusCode.UserAlreadyExists,
                        Description = StatusCode.UserAlreadyExists.GetDescriptionValue()
                    };
                }

                user = new UserAccount
                {
                    Username = model.Name,
                    Password = HashPasswordHelper.HashPassword(model.Password),
                    Email = model.Email,
                };

                await _repository.Create(user);

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>
                {
                    StatusCode = StatusCode.OK,
                    Description = "Пользователь зарегистрирован успешно",
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Register]: {ex.Message}");

                return new BaseResponse<ClaimsIdentity>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                };
            }
        }

        private ClaimsIdentity Authenticate(UserAccount user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "user")
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        public UserAccount GetUser(string username)
            => _repository.GetAll()
            .FirstOrDefault(x => x.Username == username);

    }
}
