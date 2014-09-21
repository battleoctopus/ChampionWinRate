using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using MatchHistoryNameSpace;
using MatchInfoNameSpace;

namespace ChampionWinRate
{
    // Takes in JSON strings and parses them into JSON structures.
    class Parser
    {
        // Since only the summoner ID is needed, no JSON structure is created.
        public static String GetSummonerId(String json)
        {
            System.Text.RegularExpressions.Match idLineMatch = Regex.Match(json, "\"id\":[0-9]+,");
            String idLine = idLineMatch.Groups[0].Value;
            System.Text.RegularExpressions.Match idMatch = Regex.Match(idLine, "[0-9]+");
            String id = idMatch.Groups[0].Value;
            return id;
        }

        public static MatchHistory ParseMatchHistory(String json)
        {
            MatchHistory matchHistory = JsonConvert.DeserializeObject<MatchHistory>(json);
            return matchHistory;
        }

        public static MatchInfo ParseMatchInfo(String json)
        {
            MatchInfo matchInfo = JsonConvert.DeserializeObject<MatchInfo>(json);
            return matchInfo;
        }
    }
}
