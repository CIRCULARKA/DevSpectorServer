namespace InvMan.Common.SDK.Models
{
    public class LanguageModel
    {
        public string Name { get; set; }

        public string NativeName { get; set; }

        public string Code { get; set; }

        public LanguageModel(string name, string nativeName, string code)
        {
            Name = name;
            NativeName = nativeName;
            Code = code;
        }
    }
}