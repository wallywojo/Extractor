namespace StatExtractor
{
    partial class MainPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            this.cmdExtract = new System.Windows.Forms.Button();
            this.cmdCoach = new System.Windows.Forms.Button();
            this.cmdPlayer = new System.Windows.Forms.Button();
            this.cmdTeam = new System.Windows.Forms.Button();
            this.cmdSchedule = new System.Windows.Forms.Button();
            this.cmdSchedule_P = new System.Windows.Forms.Button();
            this.cmdPlayerStats_P = new System.Windows.Forms.Button();
            this.cmdSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdExtract
            // 
            this.cmdExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdExtract.Location = new System.Drawing.Point(245, 62);
            this.cmdExtract.Margin = new System.Windows.Forms.Padding(2);
            this.cmdExtract.Name = "cmdExtract";
            this.cmdExtract.Size = new System.Drawing.Size(110, 31);
            this.cmdExtract.TabIndex = 0;
            this.cmdExtract.Text = "Extract";
            this.cmdExtract.UseVisualStyleBackColor = true;
            this.cmdExtract.Click += new System.EventHandler(this.cmdExtract_Click);
            // 
            // cmdCoach
            // 
            this.cmdCoach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCoach.Location = new System.Drawing.Point(245, 111);
            this.cmdCoach.Margin = new System.Windows.Forms.Padding(2);
            this.cmdCoach.Name = "cmdCoach";
            this.cmdCoach.Size = new System.Drawing.Size(110, 31);
            this.cmdCoach.TabIndex = 1;
            this.cmdCoach.Text = "Coach Stats";
            this.cmdCoach.UseVisualStyleBackColor = true;
            // 
            // cmdPlayer
            // 
            this.cmdPlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdPlayer.Location = new System.Drawing.Point(245, 161);
            this.cmdPlayer.Margin = new System.Windows.Forms.Padding(2);
            this.cmdPlayer.Name = "cmdPlayer";
            this.cmdPlayer.Size = new System.Drawing.Size(110, 31);
            this.cmdPlayer.TabIndex = 2;
            this.cmdPlayer.Text = "Player Stats";
            this.cmdPlayer.UseVisualStyleBackColor = true;
            // 
            // cmdTeam
            // 
            this.cmdTeam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdTeam.Location = new System.Drawing.Point(245, 210);
            this.cmdTeam.Margin = new System.Windows.Forms.Padding(2);
            this.cmdTeam.Name = "cmdTeam";
            this.cmdTeam.Size = new System.Drawing.Size(110, 31);
            this.cmdTeam.TabIndex = 3;
            this.cmdTeam.Text = "Team Stats";
            this.cmdTeam.UseVisualStyleBackColor = true;
            // 
            // cmdSchedule
            // 
            this.cmdSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdSchedule.Location = new System.Drawing.Point(245, 260);
            this.cmdSchedule.Margin = new System.Windows.Forms.Padding(2);
            this.cmdSchedule.Name = "cmdSchedule";
            this.cmdSchedule.Size = new System.Drawing.Size(110, 31);
            this.cmdSchedule.TabIndex = 4;
            this.cmdSchedule.Text = "Schedule";
            this.cmdSchedule.UseVisualStyleBackColor = true;
            // 
            // cmdSchedule_P
            // 
            this.cmdSchedule_P.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdSchedule_P.Location = new System.Drawing.Point(245, 310);
            this.cmdSchedule_P.Margin = new System.Windows.Forms.Padding(2);
            this.cmdSchedule_P.Name = "cmdSchedule_P";
            this.cmdSchedule_P.Size = new System.Drawing.Size(110, 31);
            this.cmdSchedule_P.TabIndex = 5;
            this.cmdSchedule_P.Text = "Playoff Tree";
            this.cmdSchedule_P.UseVisualStyleBackColor = true;
            // 
            // cmdPlayerStats_P
            // 
            this.cmdPlayerStats_P.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdPlayerStats_P.Location = new System.Drawing.Point(245, 359);
            this.cmdPlayerStats_P.Margin = new System.Windows.Forms.Padding(2);
            this.cmdPlayerStats_P.Name = "cmdPlayerStats_P";
            this.cmdPlayerStats_P.Size = new System.Drawing.Size(110, 31);
            this.cmdPlayerStats_P.TabIndex = 6;
            this.cmdPlayerStats_P.Text = "Playoff Player Stats";
            this.cmdPlayerStats_P.UseVisualStyleBackColor = true;
            // 
            // cmdSettings
            // 
            this.cmdSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdSettings.Location = new System.Drawing.Point(245, 409);
            this.cmdSettings.Margin = new System.Windows.Forms.Padding(2);
            this.cmdSettings.Name = "cmdSettings";
            this.cmdSettings.Size = new System.Drawing.Size(110, 31);
            this.cmdSettings.TabIndex = 7;
            this.cmdSettings.Text = "Settings";
            this.cmdSettings.UseVisualStyleBackColor = true;
            this.cmdSettings.Click += new System.EventHandler(this.cmdSettings_Click);
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::StatExtractor.Properties.Resources.EASports;
            this.ClientSize = new System.Drawing.Size(600, 486);
            this.Controls.Add(this.cmdSettings);
            this.Controls.Add(this.cmdPlayerStats_P);
            this.Controls.Add(this.cmdSchedule_P);
            this.Controls.Add(this.cmdSchedule);
            this.Controls.Add(this.cmdTeam);
            this.Controls.Add(this.cmdPlayer);
            this.Controls.Add(this.cmdCoach);
            this.Controls.Add(this.cmdExtract);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainPage";
            this.Text = "Season Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdExtract;
        private System.Windows.Forms.Button cmdCoach;
        private System.Windows.Forms.Button cmdPlayer;
        private System.Windows.Forms.Button cmdTeam;
        private System.Windows.Forms.Button cmdSchedule;
        private System.Windows.Forms.Button cmdSchedule_P;
        private System.Windows.Forms.Button cmdPlayerStats_P;
        private System.Windows.Forms.Button cmdSettings;
    }
}