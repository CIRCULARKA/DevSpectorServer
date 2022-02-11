using System.Collections.Generic;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.Service
{
    public interface ILanguageManager
    {
        LanguageModel CurrentLanguage { get; }
        
        LanguageModel DefaultLanguage { get; }
        
        List<LanguageModel> AllLanguages { get; }

        void SetLanguage(string code);

        void SetLanguage(LanguageModel model);
    }
}