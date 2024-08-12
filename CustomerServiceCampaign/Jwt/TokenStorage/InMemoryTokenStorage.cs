using System.Collections.Concurrent;

namespace CustomerServiceCampaign.API.Jwt.TokenStorage
{
    public class InMemoryTokenStorage : ITokenStorage
    {
        private static ConcurrentDictionary<string, bool> Tokens { get; }

        static InMemoryTokenStorage()
        {
            Tokens = new ConcurrentDictionary<string, bool>();
        }

        public void AddToken(string id)
        {
            int retryCount = 0;
            bool tokenAdded = false;

            while (retryCount < 3 && !tokenAdded)
            {
                tokenAdded = Tokens.TryAdd(id, true);

                if (!tokenAdded)
                {
                    retryCount++;
                    Task.Delay(60).Wait();
                }
            }

            if (!tokenAdded)
            {
                throw new InvalidOperationException("Token could not have been added after multiple tries");
            }

        }

        public bool TokenExists(string id)
        {
            bool exists = Tokens.ContainsKey(id);

            if (!exists)
            {
                return false;
            }

            return Tokens[id];
        }

        public void InvalidateToken(string id)
        {
            bool value = false;
            Tokens.Remove(id, out value);
        }

    }
}
