using System.Globalization;
using Microsoft.Extensions.Localization;
using WebUI.Services;

namespace WebUI
{
    public sealed class CustomLocalizer : StringLocalizer<GlobalViews>
    {
        public string CurrentLanguage { get; set; }
        private readonly IStringLocalizer _internalLocalizer;

        public CustomLocalizer(IStringLocalizerFactory factory, ISessionService _sessionService) : base(factory)
        {
            if (GlobalViews.Culture == null)
                GlobalViews.Culture = new CultureInfo(_sessionService.GetLanguageCookieOnlyLang());
            CurrentLanguage = GlobalViews.Culture.Name;
            _internalLocalizer = WithCulture(new CultureInfo(CurrentLanguage));
        }

        public override LocalizedString this[string name, params object[] arguments] => _internalLocalizer[name, arguments];

        public override LocalizedString this[string name] => _internalLocalizer[name];

    }
}
