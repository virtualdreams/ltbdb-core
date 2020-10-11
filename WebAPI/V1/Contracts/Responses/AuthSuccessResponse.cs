namespace ltbdb.WebAPI.V1.Contracts.Responses
{
	public class AuthSuccessResponse
	{
		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }

		public string Type { get; set; }

		public int ExpiresIn { get; set; }
	}
}