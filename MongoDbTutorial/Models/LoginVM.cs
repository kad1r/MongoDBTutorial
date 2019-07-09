using System.ComponentModel.DataAnnotations;

namespace MongoDbTutorial.Models
{
	public class LoginVM
	{
		[RegularExpression(@"(\S)+", ErrorMessage = "Please do not use spaces")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[RegularExpression(@"(\S)+", ErrorMessage = "Please do not use spaces")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}

	public class ForgotPasswordVM
	{
		[RegularExpression(@"(\S)+", ErrorMessage = "Please do not use spaces")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
	}
}
