using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Repositories;

namespace be_lspmpi.Services
{
    public class WebSettingService(IWebSettingRepo webSettingRepo) : IWebSettingService
    {
        public async Task<WebSetting> Get()
        {
            return await webSettingRepo.Get();
        }

        public async Task<ServiceResponse> Update(WebSetting webSetting)
        {
            try
            {
                await webSettingRepo.Update(webSetting);
                return new ServiceResponse { Success = true, Message = "Web settings updated successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }
    }
}
