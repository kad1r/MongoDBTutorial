using System;

namespace Model.Model
{
	public class AuthUser : User
	{
		public DateTime LastLoginDate { get; set; }
		public DateTime LastFailedLoginDate { get; set; }
		public int FailedLoginCount { get; set; }
		public DateTime NextLoginDate { get; set; }
	}
}
