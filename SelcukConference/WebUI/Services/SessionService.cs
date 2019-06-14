using System;
using System.Globalization;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using WebUI.ExtentionMethods;

namespace WebUI.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetUyeSession(Uye u)
        {
            _httpContextAccessor.HttpContext.Session.SetObject("oturumSession", u);
        }

        public Uye GetUyeSession()
        {
            return _httpContextAccessor.HttpContext.Session.GetObject<Uye>("oturumSession");
        }

        public void ClearUyeSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public void SetUyeCookie(string m, string p)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("oturumCookie", m + "-" + p, new CookieOptions { Expires = DateTimeOffset.MaxValue });
        }

        public string GetUyeCookie()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies["oturumCookie"];
        }

        public void ClerUyeCookie()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("oturumCookie");
        }

        public void SetLanguageCookie(string dil)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(dil)),
                new CookieOptions { Expires = DateTimeOffset.MaxValue }
            );
            GlobalViews.Culture = new CultureInfo(dil);
        }

        public string GetLanguageCookie()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
        }

        public string GetLanguageCookieOnlyLang()
        {
            var cook = _httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            return !string.IsNullOrEmpty(cook) ? cook.Split('|')[0].Split('=')[1] : "tr";
        }

        public void ClerLanguageCookie()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
        }
    }
}
