namespace InvMan.Common.SDK.Models
{
	/// <summary>
	/// Client-side user model
	/// </summary>
	public class User
	{
		public User(string acessToken, string login, string group)
		{
			AcessToken = acessToken;
			Login = login;
			Group = group;
		}

		public string AcessToken { get; }

		public string Login { get; }

		public string Group { get; }
	}
}
