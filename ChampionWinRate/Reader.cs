﻿using ChampionWinRate.Properties;
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
    // object as a string. A dictionary maps API keys to queues of their ten
    // most recent requests.
    class Reader
    {
        private Dictionary<String, Queue<DateTime>> keysDict = new Dictionary<string,Queue<DateTime>>();
        private const String DAWN_OF_TIME = "0001-01-01T00:00:00.000"; // arbitrary time long before any requests
        private const int MAX_REQUESTS_PER_INTERVAL = 10;
        private const double INTERVAL = 10; // measured in seconds
        private const String RATE_LIMIT_CODE = "429";
        private const String RATE_LIMIT_EXCEEDED = "rate limit exceeded";

        public Reader()
        {
            String[] keys = Resources.ApiKeys.Split(','); // API keys are stored as a resource

            foreach (String key in keys)
            {
                Queue<DateTime> timeQueue = new Queue<DateTime>();
                
                // instantiate each queue of request times
                for (int i = 0; i < MAX_REQUESTS_PER_INTERVAL; i++)
                {
                    timeQueue.Enqueue(DateTime.Parse(DAWN_OF_TIME));
                }

                keysDict[key] = timeQueue;
            }
        }

        // Sends an API request to riotgames.com. Returns an empty string if
        // Riot doesn't honor its rate limit. API key must be included in url.
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

        // Sends an API request for static data to riotgames.com. Adds the first
        // API key since static requests do not count towards the rate limit.
        public String RequestStatic(String url)
        {
            String message = Request(url + keysDict.Keys.First());
            return message;
        }

        // Tries to send an API request to riotgames.com. If all API keys have
        // reached their rate limit, the method stalls.
        public String TryRequest(String url)
        {
            // stall until an API key is under the rate limit
            while (true)
            {
                foreach (String key in keysDict.Keys)
                {
                    Queue<DateTime> timeQueue = keysDict[key];

                    if (DateTime.Now - timeQueue.Peek() > TimeSpan.FromSeconds(INTERVAL))
                    {
                        String message = Request(url + key);

                        if (message == RATE_LIMIT_EXCEEDED)
                        {
                            break;
                        }

                        timeQueue.Enqueue(DateTime.Now);
                        timeQueue.Dequeue();
                        return message;
                    }
                }
            }
        }
    }
}
