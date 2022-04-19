using System.Collections.Generic;

namespace DevSpector.UI
{
    public class BadRequestError
    {
        public string Error { get; init; }

        public IEnumerable<string> Description { get; init; }
    }
}
