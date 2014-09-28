using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChampionWinRate
{
    // Gets the API URL requests.
    class Coder
    {
        private const String HTTPS = "https://";
        private const String API = ".api.pvp.net/api/lol/";
        private const String KEY = "api_key=";

        public static String GetSummonerIdUrl(String region, String summonerName)
        {
            String getSummonerId = "/v1.4/summoner/by-name/";
            return HTTPS + region + API + region + getSummonerId + HttpUtility.UrlEncode(summonerName.Replace(" ", String.Empty)) + "?" + KEY;
        }

        public static String GetMatchHistoryUrl(String region, String summonerId, int begin, int end)
        {
            String getMatchHistory = "/v2.2/matchhistory/";
            String queues = "rankedQueues=RANKED_SOLO_5x5";
            return HTTPS + region + API + region + getMatchHistory + summonerId + "?" + queues + "&" + "beginIndex=" + begin + "&" + "endIndex=" + end + "&" + KEY;
        }

        public static String GetMatchInfoUrl(String region, int matchId)
        {
            String getMatchInfo = "/v2.2/match/";
            return HTTPS + region + API + region + getMatchInfo + matchId + "?" + KEY;
        }

        public static String GetChampionNamesUrl()
        {
            String championNames = "/v1.2/champion";
            return HTTPS + "na" + API + "static-data/" + "na" + championNames + "?" + KEY;
        }
    }
}
