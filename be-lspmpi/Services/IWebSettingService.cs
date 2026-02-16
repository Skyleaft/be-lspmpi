using be_lspmpi.Dto;
using be_lspmpi.Models;

namespace be_lspmpi.Services
{
    public interface IWebSettingService
    {
        Task<WebSetting> Get();
        Task<WebSettingPublic> GetPublic();
        Task<ServiceResponse> Update(WebSetting webSetting);
    }
}
