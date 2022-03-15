namespace DevSpector.Application.Networking
{
	public interface IIPAddressEditor
	{
		void GenerateRange(string networkAddress, int mask);
	}
}
