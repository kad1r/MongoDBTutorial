using Model.Model;
using System.Security.Principal;

namespace MongoDbTutorial.Helpers
{
	public class LoggedInUser : AuthUser, IPrincipal
	{
		public IIdentity Identity { get; private set; }

		public bool IsUserAdmin()
		{
			if (IsAdmin)
				return true;
			else
				return false;
		}

		public bool IsInRole(string role)
		{
			return true;
		}

		public string GetId()
		{
			return Id;
		}

		public LoggedInUser(string email)
		{
			Identity = new GenericIdentity(email);
		}
	}
}
