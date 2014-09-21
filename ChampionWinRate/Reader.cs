using ChampionWinRate.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChampionWinRate
{
    // Takes in a string URL request without the API key and returns the JSON
    // object as a string.
    class Reader
    {
        private String key;
        private const String NOT_FOUND_CODE = "404";
        private const String RATE_LIMIT_CODE = "429";
        private const String RATE_LIMIT_EXCEEDED = "rate limit exceeded";

        public Reader()
        {
            this.key = Resources.ApiKeys;
        }

        // Sends an API request to riotgames.com. Stalls if Riot throws an
        // error. API key must not be included.
        public String Request(String url)
        {
            using (WebClient client = new WebClient())
            {
                // stall until the API key is under the rate limit
                while (true)
                {
                    try
                    {
                        return client.DownloadString(url + key);
                    }
                    catch (WebException webException)
                    {
                        String exception = webException.ToString();

                        if (exception.Contains(RATE_LIMIT_CODE))
                        {
                            Console.WriteLine(RATE_LIMIT_EXCEEDED);
                        }
                        else if (exception.Contains(NOT_FOUND_CODE))
                        {
                            return "";
                        }
                        else
                        {
                            Console.WriteLine(exception);
                        }
                    }
                }
            }
        }
    }
}
