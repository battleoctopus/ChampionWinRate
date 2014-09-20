using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChampionWinRate
{
    public partial class Gui : Form
    {
        private Model model;
        private const int ENTER = 13;
        private const int MIN_GAME_DEFAULT = 1;
        private bool isLoaded = false;

        public Gui()
        {
            InitializeComponent();
            this.region.Text = "na";
        }

        private void load_Click(object sender, EventArgs e)
        {
            personalWin.Text = "";
            personalWin.Refresh();
            data.DataSource = null;
            data.Refresh();
            model = new Model(region.Text);

            String summonerIdUrl = Coder.GetSummonerIdUrl(region.Text, summoner.Text);

            if (model.reader.Request(summonerIdUrl).Equals(""))
            {
                System.Windows.Forms.MessageBox.Show("invalid summoner name and or region");
                return;
            }

            String summonerIdJson = model.reader.Request(summonerIdUrl);

            model.StorePersonalHistory(summoner.Text, status);
            model.StoreGlobalHistory(status);
            model.CalcChampionStats();
            model.CalcWinRates(status);
            personalWin.Text = "personal win rate: " + model.CalcPersonalWinRate().ToString("0.#") +"%";
            personalWin.Refresh();
            isLoaded = true;
            int n;
            int minGames = (int.TryParse(minGamesAnswer.Text, out n)) ? n : MIN_GAME_DEFAULT;
            minGamesAnswer.Text = minGames.ToString();
            minGamesAnswer_KeyPress("", new KeyPressEventArgs((char) ENTER));
            status.Text = "done with " + model.CountMatches() + " games";
            status.Refresh();
        }

        private void minGamesAnswer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) ENTER)
            {
                int n;

                if (!isLoaded)
                {
                    System.Windows.Forms.MessageBox.Show("data is not loaded yet");
                    return;
                }
                else if (!int.TryParse(minGamesAnswer.Text, out n))
                {
                    System.Windows.Forms.MessageBox.Show("not a whole number");
                    return;
                }
                else
                {
                    minGamesAnswer.Text = n.ToString();
                }

                DataView dataView = new DataView(model.winRates);
                String allyGamesMin = "[" + Model.ALLY_GAMES + "]" + " >= " + minGamesAnswer.Text;
                String enemyGamesMin = "[" + Model.ENEMY_GAMES + "]" + " >= " + minGamesAnswer.Text;
                dataView.RowFilter =  allyGamesMin + " AND " + enemyGamesMin;
                data.DataSource = dataView;
                data.Columns[Model.ALLY_WIN_RATE].DefaultCellStyle.Format = "0.#";
                data.Columns[Model.ENEMY_WIN_RATE].DefaultCellStyle.Format = "0.#";
                data.Refresh();
            }
        }

        private void summoner_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) ENTER)
            {
                load_Click("", new EventArgs());
            }
        }

        private void minGamesAnswer_Leave(object sender, EventArgs e)
        {
            minGamesAnswer_KeyPress("", new KeyPressEventArgs((char) ENTER));
        }
    }
}
