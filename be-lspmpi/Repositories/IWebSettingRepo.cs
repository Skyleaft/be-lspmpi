using be_lspmpi.Models;

namespace be_lspmpi.Repositories
{
    public interface IWebSettingRepo
    {
        Task<WebSetting> Get();
        Task<WebSetting> Update(WebSetting webSetting);
    }
}
