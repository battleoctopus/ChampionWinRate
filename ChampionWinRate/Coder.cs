using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChampionWinRate
{
    // Gets the URL requests.
    class Coder
    {
        private const String HTTPS = "https://";
        private const String API = ".api.pvp.net/api/lol/";
        private const String KEY = "api_key=";

        public static String GetSummonerIdUrl(String region, String summonerName)
        {
            String getSummonerId = "/v1.4/summoner/by-name/";
            String url = HTTPS + region + API + region + getSummonerId + summonerName + "?" + KEY;
            return url;
        }

        public static String GetMatchHistoryUrl(String region, String summonerId, int begin, int end)
        {
            String getMatchHistory = "/v2.2/matchhistory/";
            String queues = "rankedQueues=RANKED_SOLO_5x5";
            String url = HTTPS + region + API + region + getMatchHistory + summonerId + "?" + queues + "&" + "beginIndex=" + begin + "&" + "endIndex=" + end + "&" + KEY;
            return url;
        }

        public static String GetMatchInfoUrl(String region, int matchId)
        {
            String getMatchInfo = "/v2.2/match/";
            String url = HTTPS + region + API + region + getMatchInfo + matchId + "?" + KEY;
            return url;
        }

        public static String LookUpChampionNameUrl(String region, int championId)
        {
            String lookUpChampionName = "/v1.2/champion/";
            String url = HTTPS + "global" + API + "static-data/" + region + lookUpChampionName + championId + "?" + KEY;
            return url;
        }
    }
}
