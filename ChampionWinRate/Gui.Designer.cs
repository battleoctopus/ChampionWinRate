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
            this.go = new System.Windows.Forms.Button();
            this.personalWin = new System.Windows.Forms.TextBox();
            this.region = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.minGamesAnswer = new System.Windows.Forms.TextBox();
            this.minGamesQuery = new System.Windows.Forms.TextBox();
            this.status = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // summoner
            // 
            this.summoner.Location = new System.Drawing.Point(12, 12);
            this.summoner.Name = "summoner";
            this.summoner.Size = new System.Drawing.Size(185, 20);
            this.summoner.TabIndex = 0;
            this.summoner.Text = "summoner";
            this.summoner.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.summoner_KeyPress);
            // 
            // go
            // 
            this.go.Location = new System.Drawing.Point(257, 10);
            this.go.Name = "go";
            this.go.Size = new System.Drawing.Size(27, 23);
            this.go.TabIndex = 2;
            this.go.Text = "go";
            this.go.UseVisualStyleBackColor = true;
            this.go.Click += new System.EventHandler(this.load_Click);
            // 
            // personalWin
            // 
            this.personalWin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.personalWin.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.personalWin.Location = new System.Drawing.Point(12, 38);
            this.personalWin.Name = "personalWin";
            this.personalWin.ReadOnly = true;
            this.personalWin.Size = new System.Drawing.Size(122, 13);
            this.personalWin.TabIndex = 0;
            this.personalWin.TabStop = false;
            // 
            // region
            // 
            this.region.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.region.DropDownWidth = 50;
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
            this.region.Location = new System.Drawing.Point(203, 11);
            this.region.Name = "region";
            this.region.Size = new System.Drawing.Size(48, 21);
            this.region.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 57);
            this.dataGridView1.MaximumSize = new System.Drawing.Size(561, 242);
            this.dataGridView1.MinimumSize = new System.Drawing.Size(561, 242);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.Size = new System.Drawing.Size(561, 242);
            this.dataGridView1.TabIndex = 3;
            // 
            // minGamesAnswer
            // 
            this.minGamesAnswer.Location = new System.Drawing.Point(158, 305);
            this.minGamesAnswer.Name = "minGamesAnswer";
            this.minGamesAnswer.Size = new System.Drawing.Size(27, 20);
            this.minGamesAnswer.TabIndex = 4;
            this.minGamesAnswer.Text = "1";
            this.minGamesAnswer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.minGamesAnswer_KeyPress);
            // 
            // minGamesQuery
            // 
            this.minGamesQuery.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.minGamesQuery.Location = new System.Drawing.Point(12, 308);
            this.minGamesQuery.Name = "minGamesQuery";
            this.minGamesQuery.ReadOnly = true;
            this.minGamesQuery.Size = new System.Drawing.Size(140, 13);
            this.minGamesQuery.TabIndex = 0;
            this.minGamesQuery.TabStop = false;
            this.minGamesQuery.Text = "minimum number of games is: ";
            // 
            // status
            // 
            this.status.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.status.Location = new System.Drawing.Point(290, 14);
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Size = new System.Drawing.Size(238, 13);
            this.status.TabIndex = 0;
            this.status.TabStop = false;
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 332);
            this.Controls.Add(this.status);
            this.Controls.Add(this.minGamesQuery);
            this.Controls.Add(this.minGamesAnswer);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.region);
            this.Controls.Add(this.personalWin);
            this.Controls.Add(this.go);
            this.Controls.Add(this.summoner);
            this.Name = "Gui";
            this.ShowIcon = false;
            this.Text = "ChampionWinRate";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox summoner;
        private System.Windows.Forms.Button go;
        private System.Windows.Forms.TextBox personalWin;
        private System.Windows.Forms.ComboBox region;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox minGamesAnswer;
        private System.Windows.Forms.TextBox minGamesQuery;
        private System.Windows.Forms.TextBox status;
    }
}

