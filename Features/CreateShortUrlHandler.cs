namespace UrlShortner.Features
{
    public class CreateShortUrlHandler
    {

        private readonly Dictionary<string, string> _store;

        public CreateShortUrlHandler(Dictionary<string, string> store)
        {
            _store = store;
        }

        public string Handle(string originalUrl)
        {
            string shortCode = Guid.NewGuid().ToString().Substring(0,7);
            while (_store.ContainsKey(shortCode) ) //already exists
            {
                shortCode = Guid.NewGuid().ToString().Substring(0, 7);
            }
            _store[shortCode] = originalUrl;
            return shortCode;
        }
    }
}
