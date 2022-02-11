using System.Collections.Generic;

namespace InvMan.Desktop.Service
{
    public interface ILanguageSwitcher
    {
        LanguageInfo CurrentLanguage { get; }

        LanguageInfo DefaultLanguage { get; }

        List<LanguageInfo> AllLanguages { get; }

        void SetLanguage(string code);

        void SetLanguage(LanguageInfo model);
    }
}
