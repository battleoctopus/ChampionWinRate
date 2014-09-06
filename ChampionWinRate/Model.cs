using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchHistoryNameSpace;
using MatchInfoNameSpace;

namespace ChampionWinRate
{
    // Organizes data into dictionary with champion IDs as keys and win/loss
    // statistics as values. Creates a personal history dictionary with match
    // IDs as keys and personal match statistics as values. Creates a global
    // history dictionary with match IDs as keys and a list of global
    // participants as values.
    class Model
    {
        private Reader reader = new Reader();
        private Dictionary<int, PersonalParticipant> personalHistory = new Dictionary<int, PersonalParticipant>();
        private Dictionary<int, List<GlobalParticipant>> globalHistory = new Dictionary<int, List<GlobalParticipant>>();
        private Dictionary<int, Dictionary<Stats, int>> championStats = new Dictionary<int, Dictionary<Stats, int>>();
        private String region;
        private const int MATCH_SEARCH_LIMIT = 15; // Riot restricts the amount of match history that can be searched

        // Stores personal history in a dictionary.
        public void StorePersonalHistory(String region, String summonerName)
        {
            this.region = region;

            String summonerIdUrl = Coder.GetSummonerIdUrl(region, summonerName);
            String summonerIdJson = reader.TryRequest(summonerIdUrl);
            String summonerId = Parser.GetSummonerId(summonerIdJson);

            int begin = 0;

            // loop until there is no more match history
            while (true)
            {
                String matchHistoryUrl = Coder.GetMatchHistoryUrl(region, summonerId, begin, begin + MATCH_SEARCH_LIMIT);
                String matchHistoryJson = reader.TryRequest(matchHistoryUrl);

                // there is no more match history
                if (matchHistoryJson.Equals("{}"))
                {
                    break;
                }

                MatchHistory matchHistory = Parser.ParseMatchHistory(matchHistoryJson);
                foreach (Match match in matchHistory.matches)
                {
                    MatchHistoryNameSpace.Participant participant = match.participants[0];
                    personalHistory[match.matchId] = new PersonalParticipant(match.participants[0].teamId, participant.stats.winner, participant.championId);
                }

                begin += MATCH_SEARCH_LIMIT;
            }
        }

        // Stores global history in a dictionary.
        public void StoreGlobalHistory()
        {
            foreach (int matchId in personalHistory.Keys)
            {
                String matchInfoUrl = Coder.GetMatchInfoUrl(region, matchId);
                String matchInfoJson = reader.TryRequest(matchInfoUrl);
                MatchInfo matchInfo = Parser.ParseMatchInfo(matchInfoJson);
                globalHistory[matchId] = new List<GlobalParticipant>();

                foreach (MatchInfoNameSpace.Participant participant in matchInfo.participants)
                {
                    globalHistory[matchId].Add(new GlobalParticipant(participant.teamId, participant.stats.winner, participant.championId));
                }
            }
        }

        // Tallies the appropriate win/loss and ally/enemy statistic in a
        // dictionary.
        private void AddTally(int championId, Stats stat)
        {
            // add champion to dictionary if not already contained
            if (!championStats.Keys.Contains(championId))
            {
                championStats[championId] = new Dictionary<Stats, int>();

                // initialize all win/loss and ally/enemy statistics to zero
                foreach (Stats statEnum in (Stats[]) Enum.GetValues(typeof(Stats)))
                {
                    championStats[championId][statEnum] = 0;
                }
            }

            championStats[championId][stat] += 1;
        }

        // Calculates champion statistics from personal and global history
        // dictionaries.
        public void CalcChampionStats()
        {
            foreach (int matchId in personalHistory.Keys)
            {
                PersonalParticipant personalParticipant = personalHistory[matchId];

                foreach (GlobalParticipant globalParticipant in globalHistory[matchId])
                {
                    if (globalParticipant.teamId == personalParticipant.teamId)
                    {
                        if (globalParticipant.championId != personalParticipant.championId)
                        {
                            AddTally(globalParticipant.championId, globalParticipant.isWin ? Stats.AllyWin : Stats.AllyLoss);
                        }
                    }
                    else
                    {
                        AddTally(globalParticipant.championId, globalParticipant.isWin ? Stats.EnemyWin : Stats.EnemyLoss);
                    }
                }
            }
        }
    }

    public struct PersonalParticipant
    {
        public int teamId;
        public bool isWin;
        public int championId;

        public PersonalParticipant(int teamId, bool isWin, int championId)
        {
            this.teamId = teamId;
            this.isWin = isWin;
            this.championId = championId;
        }
    }

    public struct GlobalParticipant
    {
        public int teamId;
        public bool isWin;
        public int championId;

        public GlobalParticipant(int teamId, bool isWin, int championId)
        {
            this.teamId = teamId;
            this.isWin = isWin;
            this.championId = championId;
        }
    }

    // statistics for win/loss and ally/enemy
    enum Stats
    {
        AllyWin, AllyLoss, EnemyWin, EnemyLoss
    }
}
