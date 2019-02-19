namespace ltbdb
{
	public class Settings
	{
		/// <summary>
		/// MySql connection string.
		/// </summary>
		public string ConnectionString { get; set; } = "Server=localhost;Database=ltbdb;User=ltbdb;Password=ltbdb";

		/// <summary>
		/// Items per page to display.
		/// </summary>
		public int ItemsPerPage { get; set; } = 18;

		/// <summary>
		/// Recent items to display.
		/// </summary>
		public int RecentItems { get; set; } = 18;

		/// <summary>
		/// Storage path, where the images be saved.
		/// Can be a relative path or a full qualified url.
		/// </summary>
		public string Storage { get; set; } = "./wwwroot/images";

		/// <summary>
		/// Path to image, if no cover exists.
		/// </summary>
		public string NoImage { get; set; } = "/content/no-image.png";

		/// <summary>
		/// CDN path, where the images are located.
		/// </summary>
		public string CDNPath { get; set; } = "/images/";

		/// <summary>
		/// Path to GraphicsMagick to process the uploaded images.
		/// </summary>
		public string GraphicsMagick { get; set; } = "gm";

		/// <summary>
		/// The username to login.
		/// </summary>
		public string Username { get; set; } = "";

		/// <summary>
		/// The password to login.
		/// </summary>
		public string Password { get; set; } = "";

		/// <summary>
		/// Path to keystore directory.
		/// </summary>
		public string KeyStore { get; set; } = "";

		/// <summary>
		/// Security key for jwt bearer token.
		/// </summary>
		/// <returns></returns>
		public string SecurityKey { get; set; } = "";
	}
}