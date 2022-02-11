namespace InvMan.Desktop.Service
{
    public class LanguageInfo
    {
        public string Name { get; set; }

        public string NativeName { get; set; }

        public string Code { get; set; }

        public LanguageInfo(string name, string nativeName, string code)
        {
            Name = name;
            NativeName = nativeName;
            Code = code;
        }
    }
}
