namespace MongoDbTutorial.Models
{
    public class JsResult
    {
        public bool HasError { get; set; } = false;
        public object ErrorList { get; set; }
        public string ErrorMessage { get; set; }
    }
}
