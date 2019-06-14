using Model.Model;
using System.Collections.Generic;

namespace MongoDbTutorial.Models
{
    public class UserVM
    {
        public User User { get; set; }
        public MongoDB.Driver.IAsyncCursor<User> UserModel { get; set; }
        public IEnumerable<User> List { get; set; }
    }
}
