using Model.Model;
using MongoDB.Driver;
using System.Collections.Generic;

namespace MongoDbTutorial.Models
{
    public class UserVM
    {
        public User User { get; set; }
        public IAsyncCursor<User> UserModel { get; set; }
        public IEnumerable<User> List { get; set; }
    }
}
