using DataAccess.Entities;

namespace WebUI.Services
{
    public interface ISessionService
    {
        void SetUyeSession(Uye u);
        Uye GetUyeSession();
        void ClearUyeSession();

        void SetUyeCookie(string m, string p);
        string GetUyeCookie();
        void ClerUyeCookie();

        void SetLanguageCookie(string dil);
        string GetLanguageCookie();
        string GetLanguageCookieOnlyLang();
        void ClerLanguageCookie();
    }
}