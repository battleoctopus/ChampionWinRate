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
            status.Text = "";
            status.Refresh();
            model = new Model(region.Text);
            status.Text = "finding all ranked solo and duo games";
            status.Refresh();

            String summonerIdUrl = Coder.GetSummonerIdUrl(region.Text, summoner.Text);

            if (model.reader.TryRequest(summonerIdUrl).Equals(""))
            {
                System.Windows.Forms.MessageBox.Show("invalid summoner name and or region");
                return;
            }

            String summonerIdJson = model.reader.TryRequest(summonerIdUrl);

            model.StorePersonalHistory(summoner.Text);
            status.Text = "Found all games. Loading game data.";
            status.Refresh();
            model.StoreGlobalHistory(status);
            status.Text = "Loaded all game data. Tallying wins and losses.";
            status.Refresh();
            model.CalcChampionStats();
            status.Text = "Tallied all game data. Calculating win rates.";
            status.Refresh();
            model.CalcWinRates();
            personalWin.Text = "personal win rate: " + model.CalcPersonalWinRate().ToString("#.#") +"%";
            personalWin.Refresh();
            isLoaded = true;
            minGamesAnswer_KeyPress("", new KeyPressEventArgs((char) ENTER));
            status.Text = "done with " + model.CountMatches() + " games";
            status.Refresh();
        }

        private void minGamesAnswer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) ENTER)
            {
                Regex regex = new Regex("[0-9]+");

                if (!isLoaded)
                {
                    System.Windows.Forms.MessageBox.Show("data is not loaded yet");
                    return;
                } else if (!regex.IsMatch(minGamesAnswer.Text))
                {
                    System.Windows.Forms.MessageBox.Show("not a whole number");
                    return;
                }

                DataView dataView = new DataView(model.winRates);
                String allyGamesMin = "[" + Model.ALLY_GAMES + "]" + " >= " + minGamesAnswer.Text;
                String enemyGamesMin = "[" + Model.ENEMY_GAMES + "]" + " >= " + minGamesAnswer.Text;
                dataView.RowFilter =  allyGamesMin + " AND " + enemyGamesMin;
                data.DataSource = dataView;
                data.Columns[Model.ALLY_WIN_RATE].DefaultCellStyle.Format = "#.#";
                data.Columns[Model.ENEMY_WIN_RATE].DefaultCellStyle.Format = "#.#";
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
    }
}
