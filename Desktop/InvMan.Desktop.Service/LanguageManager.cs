using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.Service
{
    public class LanguageManager : ILanguageManager
    {
        public LanguageManager()
        {
            _availableLanguages = GetAvailableLanguages();
            DefaultLanguage = createLanguageModel(CultureInfo.GetCultureInfo("ru"));
        }

        private Dictionary<string, LanguageModel> _availableLanguages { get; }
        public LanguageModel CurrentLanguage => createLanguageModel(Thread.CurrentThread.CurrentUICulture);

        public LanguageModel DefaultLanguage { get; set; }
        public List<LanguageModel> AllLanguages => new(_availableLanguages.Values);

        public void SetLanguage(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException($"{nameof(code)} can't be empty");

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(code);

            // TODO: Update settings here when implemented
        }

        public void SetLanguage(LanguageModel model)
        {
            SetLanguage(model.Code);
        }

        private Dictionary<string, LanguageModel> GetAvailableLanguages()
        {
            var locals = new List<string> { "en", "ru" };
            return locals.Select(l => createLanguageModel(new CultureInfo(l))).ToDictionary(lm => lm.Code, lm => lm);
        }

        private LanguageModel createLanguageModel(CultureInfo cultureInfo)
        {
            return cultureInfo is null
                ? DefaultLanguage
                : new LanguageModel(cultureInfo.EnglishName, cultureInfo.NativeName,
                    cultureInfo.TwoLetterISOLanguageName);
        }
    }
}