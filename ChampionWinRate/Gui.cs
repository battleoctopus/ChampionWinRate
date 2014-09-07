using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChampionWinRate
{
    public partial class Gui : Form
    {
        private Model model;
        private const int ENTER = 13;

        public Gui()
        {
            InitializeComponent();
        }

        private void load_Click(object sender, EventArgs e)
        {
            model = new Model(region.Text);
            model.StorePersonalHistory(summoner.Text);
            model.StoreGlobalHistory();
            model.CalcChampionStats();
            model.CalcWinRates();
            personalWin.Text = "Personal Win Rate: " + model.CalcPersonalWinRate() + "%";
            dataGridView1.DataSource = model.winRates;
        }

        private void minGamesAnswer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) ENTER)
            {
                DataView dataView = new DataView(model.winRates);
                String allyGamesMin = "[" + Model.ALLY_GAMES + "]" + " >= " + minGamesAnswer.Text;
                String enemyGamesMin = "[" + Model.ENEMY_GAMES + "]" + " >= " + minGamesAnswer.Text;
                dataView.RowFilter =  allyGamesMin + " AND " + enemyGamesMin;
                dataGridView1.DataSource = dataView;
                dataGridView1.Refresh();
            }
        }
    }
}
