namespace InvMan.Common.SDK.Models
{
	/// <summary>
	/// Client-side user model
	/// </summary>
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
