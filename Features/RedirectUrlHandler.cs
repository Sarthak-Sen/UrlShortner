namespace UrlShortner.Features
{
    public class RedirectUrlHandler
    {
        private readonly Dictionary<string, string> _store;

        public RedirectUrlHandler(Dictionary<string, string> store)
        {
            _store = store;
        }

        public string? Handle(string shortCode)
        {
            return _store.TryGetValue(shortCode, out var value) ? value : null;
        }
    }
}
