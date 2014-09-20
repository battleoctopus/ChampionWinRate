using ChampionWinRate.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChampionWinRate
{
    // Takes in a string URL request without the API key and returns the JSON
    // object as a string.
    class Reader
    {
        private String key;
        private const String RATE_LIMIT_CODE = "429";
        private const String RATE_LIMIT_EXCEEDED = "rate limit exceeded";

        public Reader()
        {
            this.key = Resources.ApiKeys;
        }

        // Sends an API request to riotgames.com. Returns an empty string if
        // Riot returns an error. API key must be included in url.
        private String Request(String url)
        {
            WebClient client = new WebClient();

            // Riot doesn't always honor its rate limit
            try
            {
                Stream stream = client.OpenRead(url);
                StreamReader reader = new StreamReader(stream);
                String message = reader.ReadToEnd();
                return message;
            }
            catch (WebException webException)
            {
                if (webException.ToString().Contains(RATE_LIMIT_CODE))
                {
                    Console.WriteLine(RATE_LIMIT_EXCEEDED);
                    return RATE_LIMIT_EXCEEDED;
                }
                else
                {
                    Console.WriteLine(webException.ToString());
                }

                return "";
            }
        }

        // Sends an API request for static data to riotgames.com. Static
        // requests do not count towards the rate limit.
        public String RequestStatic(String url)
        {
            return Request(url + key);
        }

        // Tries to send an API request to riotgames.com, stalling if the rate
        // limit is reached.
        public String TryRequest(String url)
        {
            // stall until the API key is under the rate limit
            while (true)
            {
                String message = Request(url + key);

                if (message == RATE_LIMIT_EXCEEDED | message == "")
                {
                    continue;
                }

                return message;
            }
        }
    }
}
