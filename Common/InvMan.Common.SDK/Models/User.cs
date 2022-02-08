namespace InvMan.Common.SDK.Models
{
	/// <summary>
	/// Client-side user model
	/// </summary>
	public class User
	{
		public User(string accessToken, string login, string group)
		{
			AccessToken = accessToken;
			Login = login;
			Group = group;
		}

		public string AccessToken { get; }

		public string Login { get; }

		public string Group { get; }
	}
}
