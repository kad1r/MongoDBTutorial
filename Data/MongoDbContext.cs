using MongoDB.Driver;
using System.Configuration;

namespace Data
{
    public class MongoDbContext<T> where T : class
    {
        public IMongoDatabase db { get; set; }
        public IMongoCollection<T> collection { get; private set; }

        public MongoDbContext()
        {
            var mongoClient = new MongoClient(ConfigurationManager.AppSettings["MongoDbHost"]);

            db = mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongoDBName"]);
            collection = db.GetCollection<T>(MapModelNameToDatabase(typeof(T).Name));
        }

        private static string MapModelNameToDatabase(string modelName)
        {
            if (modelName.EndsWith("y"))
            {
                modelName = modelName.Replace("y", "ies");
            }
            else
            {
                modelName += "s";
            }

            return modelName;
        }
    }
}
