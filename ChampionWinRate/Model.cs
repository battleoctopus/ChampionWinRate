using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchHistoryNameSpace;
using MatchInfoNameSpace;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ChampionWinRate
{
    // Organizes data into dictionary with champion IDs as keys and win/loss
    // statistics as values. Creates a personal history dictionary with match
    // IDs as keys and personal match statistics as values. Creates a global
    // history dictionary with match IDs as keys and a list of global
    // participants as values. Creates a champion stats dictionary with champion
    // IDs as keys and dictionaries with outcomes as keys and tallies as values
    // as values. Creates a champion names dictionary with champion IDs as keys
    // and champion names as values.
    class Model
    {
        public Reader reader;
        private Dictionary<int, PersonalParticipant> personalHistory = new Dictionary<int, PersonalParticipant>();
        private Dictionary<int, List<GlobalParticipant>> globalHistory = new Dictionary<int, List<GlobalParticipant>>();
        private Dictionary<int, Dictionary<Stats, int>> championStats = new Dictionary<int, Dictionary<Stats, int>>();
        private Dictionary<int, String> championNames = new Dictionary<int, String>();
        public DataTable winRates = new DataTable();
        private String region;
        private const int MATCH_SEARCH_LIMIT = 15; // Riot restricts the amount of match history that can be searched

        public const String ALLY_GAMES = "Ally Games";
        public const String ENEMY_GAMES = "Enemy Games";
        public const String ALLY_WIN_RATE = "Ally Win %";
        public const String ENEMY_WIN_RATE = "Enemy Win %";

        public Model (String region)
        {
            reader = new Reader();
            this.region = region;

            // populate championNames
            String champions = reader.Request(Coder.GetChampionNamesUrl(region));
            Regex regexId = new Regex("\"id\":(\\d+)");
            MatchCollection idMatches = regexId.Matches(champions);
            Regex regexName = new Regex("\"name\":\"([^\"]+)\"");
            MatchCollection nameMatches = regexName.Matches(champions);

            for (int i = 0; i < idMatches.Count; i++)
            {
                int id = Convert.ToInt32(idMatches[i].Groups[1].Value);
                String name = nameMatches[i].Groups[1].Value;
                championNames[id] = name;
            }
        }

        // Stores personal history in a dictionary.
        public void StorePersonalHistory(String summonerName, TextBox status)
        {
            String summonerIdUrl = Coder.GetSummonerIdUrl(region, summonerName);
            String summonerIdJson = reader.Request(summonerIdUrl);
            String summonerId = Parser.GetSummonerId(summonerIdJson);

            int matchNumber = 0;

            // loops until there is no more match history
            while (true)
            {
                String matchHistoryUrl = Coder.GetMatchHistoryUrl(region, summonerId, matchNumber, matchNumber + MATCH_SEARCH_LIMIT);
                String matchHistoryJson = reader.Request(matchHistoryUrl);

                // there is no more match history
                if (matchHistoryJson.Equals("{}") | matchHistoryJson.Equals(String.Empty))
                {
                    break;
                }

                MatchHistory matchHistory = Parser.ParseMatchHistory(matchHistoryJson);

                status.Text = (matchNumber + matchHistory.matches.Count) + " games found";
                status.Refresh();
                
                foreach (MatchHistoryNameSpace.Match match in matchHistory.matches)
                {
                    MatchHistoryNameSpace.Participant participant = match.participants[0];
                    personalHistory[match.matchId] = new PersonalParticipant(match.participants[0].teamId, participant.stats.winner, participant.championId);
                }

                matchNumber += MATCH_SEARCH_LIMIT;
            }
        }

        // Stores global history in a dictionary.
        public void StoreGlobalHistory(TextBox status)
        {
            int matchCount = 1;

            foreach (int matchId in personalHistory.Keys)
            {
                status.Text = "Found all games. Loading game data " + matchCount + "/" + personalHistory.Keys.Count + ".";
                status.Refresh();
                String matchInfoUrl = Coder.GetMatchInfoUrl(region, matchId);
                String matchInfoJson = reader.Request(matchInfoUrl);
                MatchInfo matchInfo = Parser.ParseMatchInfo(matchInfoJson);
                globalHistory[matchId] = new List<GlobalParticipant>();

                foreach (MatchInfoNameSpace.Participant participant in matchInfo.participants)
                {
                    globalHistory[matchId].Add(new GlobalParticipant(participant.teamId, participant.stats.winner, participant.championId));
                }

                matchCount += 1;
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
        public void CalcWinRates(TextBox status)
        {
            winRates.Columns.Add("Champion", typeof(String));
            winRates.Columns.Add(ALLY_GAMES, typeof(int));
            winRates.Columns.Add(ALLY_WIN_RATE, typeof(double));
            winRates.Columns.Add(ENEMY_WIN_RATE, typeof(double));
            winRates.Columns.Add(ENEMY_GAMES, typeof(int));
            
            foreach (DataColumn dataColumn in winRates.Columns)
            {
                dataColumn.ReadOnly = true;
            }

            int championCounter = 1;

            foreach (int championId in championStats.Keys)
            {
                championCounter++;
                Dictionary<Stats, int> stats = championStats[championId];

                String championName = championNames[championId];
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

        public int CountMatches()
        {
            int count = personalHistory.Keys.Count;
            return count;
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
