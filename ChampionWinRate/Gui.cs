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
        private const int ENTER = 13; // char of "enter" key
        private const int MIN_GAME_DEFAULT = 1;
        private bool isLoaded = false;

        public Gui()
        {
            InitializeComponent();
            this.region.Text = "na";
        }

        // Event: go is clicked. Gets data for summoner and region and displays
        //        it in data.
        private void go_Click(object sender, EventArgs e)
        {
            personalWin.Text = String.Empty;
            personalWin.Refresh();

            status.Text = String.Empty;
            status.Refresh();

            data.DataSource = null;
            data.Refresh();

            int n;
            int minGamesInt = (int.TryParse(minGames.Text, out n)) ? n : MIN_GAME_DEFAULT;
            minGames.Text = minGamesInt.ToString();
            minGames.Refresh();

            model = new Model(region.Text);
            String summonerIdUrl = Coder.GetSummonerIdUrl(region.Text, summoner.Text);

            // invalid summoner name and or region
            if (model.reader.Request(summonerIdUrl).Equals(String.Empty))
            {
                System.Windows.Forms.MessageBox.Show("invalid summoner name and or region");
                return;
            }

            String summonerIdJson = model.reader.Request(summonerIdUrl);

            model.StorePersonalHistory(summoner.Text, status);
            model.StoreGlobalHistory(status);
            model.CalcChampionStats();
            model.CalcWinRates(status);
            isLoaded = true;
            minGames_KeyPress(String.Empty, new KeyPressEventArgs((char) ENTER));

            double personalWinRate = model.CalcPersonalWinRate();

            if (double.IsNaN(personalWinRate))
            {
                personalWin.Text = "no ranked games played";
            }
            else
            {
                personalWin.Text = "personal win rate: " + personalWinRate.ToString("0.#") +"%";
            }

            personalWin.Refresh();

            status.Text = "done with " + model.CountMatches() + " games";
            status.Refresh();
        }

        // Event: minGames has a key press. If "enter" was pressed, filters data
        //        based on minGames.
        private void minGames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) ENTER)
            {
                int n;

                if (!isLoaded)
                {
                    System.Windows.Forms.MessageBox.Show("data is not loaded yet");
                    return;
                }
                else if (!int.TryParse(minGames.Text, out n))
                {
                    System.Windows.Forms.MessageBox.Show("not a whole number");
                    minGames.Select();
                    return;
                }
                else
                {
                    minGames.Text = n.ToString();
                }

                DataView dataView = new DataView(model.winRates);

                String allyGamesMin = "[" + Model.ALLY_GAMES + "]" + " >= " + minGames.Text;
                String enemyGamesMin = "[" + Model.ENEMY_GAMES + "]" + " >= " + minGames.Text;
                dataView.RowFilter =  allyGamesMin + " AND " + enemyGamesMin;

                data.DataSource = dataView;
                data.Columns[Model.ALLY_WIN_RATE].DefaultCellStyle.Format = "0.#";
                data.Columns[Model.ENEMY_WIN_RATE].DefaultCellStyle.Format = "0.#";
                data.Refresh();
            }
        }

        // Event: summoner has a key press. If "enter" was pressed, go_Click()
        //        is called.
        private void summoner_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) ENTER)
            {
                go_Click(String.Empty, new EventArgs());
            }
        }

        // Event: minGames loses focus. minGames_KeyPress() is called.
        private void minGames_Leave(object sender, EventArgs e)
        {
            minGames_KeyPress(String.Empty, new KeyPressEventArgs((char) ENTER));
        }
    }
}
