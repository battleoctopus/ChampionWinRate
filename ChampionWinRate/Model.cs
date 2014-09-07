using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchHistoryNameSpace;
using MatchInfoNameSpace;
using System.Data;

namespace ChampionWinRate
{
    // Organizes data into dictionary with champion IDs as keys and win/loss
    // statistics as values. Creates a personal history dictionary with match
    // IDs as keys and personal match statistics as values. Creates a global
    // history dictionary with match IDs as keys and a list of global
    // participants as values.
    class Model
    {
        private Reader reader;
        private Dictionary<int, PersonalParticipant> personalHistory = new Dictionary<int, PersonalParticipant>();
        private Dictionary<int, List<GlobalParticipant>> globalHistory = new Dictionary<int, List<GlobalParticipant>>();
        private Dictionary<int, Dictionary<Stats, int>> championStats = new Dictionary<int, Dictionary<Stats, int>>();
        public DataTable winRates = new DataTable();
        private String region;
        private const int MATCH_SEARCH_LIMIT = 15; // Riot restricts the amount of match history that can be searched
        public const String ALLY_GAMES = "Ally Games";
        public const String ENEMY_GAMES = "Enemy Games";

        public Model (String region)
        {
            reader = new Reader();
            this.region = region;
        }

        // Stores personal history in a dictionary.
        public void StorePersonalHistory(String summonerName)
        {
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

        // Calculates win rates for each champion from champion statistics
        // dictionary.
        public void CalcWinRates()
        {
            winRates.Columns.Add("Champion", typeof(String));
            winRates.Columns.Add(ALLY_GAMES, typeof(int));
            winRates.Columns.Add("Ally Win %", typeof(double));
            winRates.Columns.Add("Enemy Win %", typeof(double));
            winRates.Columns.Add(ENEMY_GAMES, typeof(int));

            foreach (int championId in championStats.Keys)
            {
                String championName = LookUpChampionName(championId);
                Dictionary<Stats, int> stats = championStats[championId];
                double allyGames = stats[Stats.AllyWin] + stats[Stats.AllyLoss];
                double allyWin = 100d * stats[Stats.AllyWin] / (stats[Stats.AllyWin] + stats[Stats.AllyLoss]);
                double enemyWin = 100d * stats[Stats.EnemyWin] / (stats[Stats.EnemyWin] + stats[Stats.EnemyLoss]);
                double enemyGames = stats[Stats.EnemyWin] + stats[Stats.EnemyLoss];
                winRates.Rows.Add(championName, allyGames, allyWin, enemyWin, enemyGames);
            }
        }

        // Calculates personal win rate from personal history dictionary.
        public double CalcPersonalWinRate()
        {
            int wins = 0;
            int games = 0;

            foreach (int matchId in personalHistory.Keys)
            {
                games += 1;
                PersonalParticipant personalParticipant = personalHistory[matchId];
                
                if (personalParticipant.isWin)
                {
                    wins += 1;
                }
            }

            return 100d * wins / games;
        }

        // Looks up the champion name given the champion ID.
        private String LookUpChampionName(int championId)
        {
            String url = Coder.LookUpChampionNameUrl(region, championId);
            String json = reader.RequestStatic(url);
            ChampionInfo championInfo = Parser.ParseChampionInfo(json);
            String championName = championInfo.name;
            return championName;
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
