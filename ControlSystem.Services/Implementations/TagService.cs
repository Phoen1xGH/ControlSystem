using ControlSystem.DAL.Interfaces;
using ControlSystem.DAL.Repositories;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Response;
using ControlSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlSystem.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly ILogger<UserAccountService> _logger;

        private IRepository<Tag> _tagRepository;

        public TagService(
            ILogger<UserAccountService> logger,
            IRepository<Tag> tagRepository)
        {
            _logger = logger;
            _tagRepository = tagRepository;
        }

        public async Task<BaseResponse<bool>> CreateTag(Tag tag)
        {
            try
            {
                await (_tagRepository as TagsRepository)!.Create(tag);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateTag]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteTag(int tagId)
        {
            try
            {
                var tag = await _tagRepository.GetAll().FirstOrDefaultAsync(x => x.Id == tagId);

                if (tag == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.TagNotFound,
                        Description = StatusCode.TagNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await (_tagRepository as TagsRepository)!.Delete(tag);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteTag]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> EditTag(int tagId, Tag newTagData)
        {
            try
            {
                var tag = await _tagRepository.GetAll().FirstOrDefaultAsync(x => x.Id == tagId);

                if (tag == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.TagNotFound,
                        Description = StatusCode.TagNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                tag.Name = newTagData.Name;
                tag.ColorHex = newTagData.ColorHex;

                await _tagRepository.Update(tag);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[EditTag]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public BaseResponse<List<Tag>> GetAllTags()
        {
            try
            {
                var tags = _tagRepository.GetAll().ToList();

                return new BaseResponse<List<Tag>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = tags
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetAllTags]: {ex.Message}");

                return new BaseResponse<List<Tag>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }
    }
}
