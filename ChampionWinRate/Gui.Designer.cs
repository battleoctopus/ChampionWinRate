namespace ChampionWinRate
{
    partial class Gui
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.summoner = new System.Windows.Forms.TextBox();
            this.load = new System.Windows.Forms.Button();
            this.personalWin = new System.Windows.Forms.TextBox();
            this.region = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.minGamesAnswer = new System.Windows.Forms.TextBox();
            this.minGamesQuery = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // summoner
            // 
            this.summoner.Location = new System.Drawing.Point(12, 12);
            this.summoner.Name = "summoner";
            this.summoner.Size = new System.Drawing.Size(100, 20);
            this.summoner.TabIndex = 0;
            this.summoner.Text = "Summoner";
            // 
            // load
            // 
            this.load.Location = new System.Drawing.Point(245, 12);
            this.load.Name = "load";
            this.load.Size = new System.Drawing.Size(75, 23);
            this.load.TabIndex = 2;
            this.load.Text = "Load";
            this.load.UseVisualStyleBackColor = true;
            this.load.Click += new System.EventHandler(this.load_Click);
            // 
            // personalWin
            // 
            this.personalWin.Enabled = false;
            this.personalWin.Location = new System.Drawing.Point(13, 39);
            this.personalWin.Name = "personalWin";
            this.personalWin.ReadOnly = true;
            this.personalWin.Size = new System.Drawing.Size(201, 20);
            this.personalWin.TabIndex = 3;
            this.personalWin.Text = "Personal Win Rate:";
            // 
            // region
            // 
            this.region.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.region.FormattingEnabled = true;
            this.region.Items.AddRange(new object[] {
            "br",
            "eune",
            "euw",
            "kr",
            "lan",
            "las",
            "na",
            "oce",
            "ru",
            "tr"});
            this.region.Location = new System.Drawing.Point(118, 12);
            this.region.Name = "region";
            this.region.Size = new System.Drawing.Size(50, 21);
            this.region.TabIndex = 4;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(40, 128);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.Size = new System.Drawing.Size(561, 360);
            this.dataGridView1.TabIndex = 5;
            // 
            // minGamesAnswer
            // 
            this.minGamesAnswer.Location = new System.Drawing.Point(198, 104);
            this.minGamesAnswer.Name = "minGamesAnswer";
            this.minGamesAnswer.Size = new System.Drawing.Size(100, 20);
            this.minGamesAnswer.TabIndex = 7;
            this.minGamesAnswer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.minGamesAnswer_KeyPress);
            // 
            // minGamesQuery
            // 
            this.minGamesQuery.Location = new System.Drawing.Point(40, 104);
            this.minGamesQuery.Name = "minGamesQuery";
            this.minGamesQuery.ReadOnly = true;
            this.minGamesQuery.Size = new System.Drawing.Size(152, 20);
            this.minGamesQuery.TabIndex = 8;
            this.minGamesQuery.Text = "Minimum number of games is: ";
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 542);
            this.Controls.Add(this.minGamesQuery);
            this.Controls.Add(this.minGamesAnswer);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.region);
            this.Controls.Add(this.personalWin);
            this.Controls.Add(this.load);
            this.Controls.Add(this.summoner);
            this.Name = "Gui";
            this.Text = "ChampionWinRate";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox summoner;
        private System.Windows.Forms.Button load;
        private System.Windows.Forms.TextBox personalWin;
        private System.Windows.Forms.ComboBox region;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox minGamesAnswer;
        private System.Windows.Forms.TextBox minGamesQuery;
    }
}

