using DevSpector.Application.Networking.Enumerations;

namespace DevSpector.Application.Networking
{
    public interface IIPValidator
    {
        bool Matches(string value, IPProtocol protocolVersion);
    }
}
