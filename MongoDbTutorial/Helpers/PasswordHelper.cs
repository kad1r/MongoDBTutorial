using System;
using System.Security.Cryptography;

namespace MongoDbTutorial.Helpers
{
	public class PasswordHelper
	{
		/// <summary>
		/// Crypt string to md5 and then sha and convert it to base64.
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public static string HashPassword(string password)
		{
			var md5Service = new MD5CryptoServiceProvider();
			var shaService = new SHA1CryptoServiceProvider();
			var bytes = System.Text.Encoding.UTF8.GetBytes(password);
			var hashedPassword = md5Service.ComputeHash(bytes);

			hashedPassword = shaService.ComputeHash(hashedPassword);

			return Convert.ToBase64String(hashedPassword);
		}
	}
}
