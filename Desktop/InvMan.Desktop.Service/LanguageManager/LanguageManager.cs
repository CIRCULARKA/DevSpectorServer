using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace InvMan.Desktop.Service
{
    public class LanguageSwitcher : ILanguageSwitcher
    {
        private Dictionary<string, LanguageInfo> _availableLanguages { get; }

        public LanguageSwitcher()
        {
            _availableLanguages = GetAvailableLanguages();
            DefaultLanguage = CreateLanguageInfo(CultureInfo.GetCultureInfo("ru"));
        }

        public LanguageInfo CurrentLanguage => CreateLanguageInfo(Thread.CurrentThread.CurrentUICulture);

        public LanguageInfo DefaultLanguage { get; set; }

        public List<LanguageInfo> AllLanguages => new(_availableLanguages.Values);

        public void SetLanguage(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException($"{nameof(code)} can't be empty");

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(code);

            // TODO: Update settings here when implemented
        }

        public void SetLanguage(LanguageInfo model) =>
            SetLanguage(model.Code);

        private Dictionary<string, LanguageInfo> GetAvailableLanguages()
        {
            var locals = new List<string> { "en", "ru" };
            return locals.Select(
                l => CreateLanguageInfo(new CultureInfo(l))
            ).ToDictionary(lm => lm.Code, lm => lm);
        }

        private LanguageInfo CreateLanguageInfo(CultureInfo cultureInfo)
        {
            return cultureInfo is null ? DefaultLanguage :
                new LanguageInfo(
                    cultureInfo.EnglishName,
                    cultureInfo.NativeName,
                    cultureInfo.TwoLetterISOLanguageName
                );
        }
    }
}
