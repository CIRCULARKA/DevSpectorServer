namespace InvMan.Common.SDK.Models
{
	public class User
	{
		public User(string acessToken, string login)
		{
			AcessToken = acessToken;
			Login = login;
		}

		public string AcessToken { get; }

		public string Login { get; }
	}
}
