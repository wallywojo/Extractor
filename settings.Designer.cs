namespace StatExtractor
{
    partial class settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(settings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdClear = new System.Windows.Forms.Button();
            this.pTeam2 = new System.Windows.Forms.PictureBox();
            this.pTeam1 = new System.Windows.Forms.PictureBox();
            this.cmbTeams = new System.Windows.Forms.ComboBox();
            this.oleDbSelectCommand1 = new System.Data.OleDb.OleDbCommand();
            this.dbConnection = new System.Data.OleDb.OleDbConnection();
            this.oleDbInsertCommand1 = new System.Data.OleDb.OleDbCommand();
            this.oleDbUpdateCommand1 = new System.Data.OleDb.OleDbCommand();
            this.oleDbDeleteCommand1 = new System.Data.OleDb.OleDbCommand();
            this.dbAdapter = new System.Data.OleDb.OleDbDataAdapter();
            this.cmdClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pTeam2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTeam1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdClear);
            this.groupBox1.Controls.Add(this.pTeam2);
            this.groupBox1.Controls.Add(this.pTeam1);
            this.groupBox1.Controls.Add(this.cmbTeams);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(820, 181);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clear Stats";
            // 
            // cmdClear
            // 
            this.cmdClear.Location = new System.Drawing.Point(365, 124);
            this.cmdClear.Margin = new System.Windows.Forms.Padding(2);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(88, 25);
            this.cmdClear.TabIndex = 9;
            this.cmdClear.Text = "Clear Stats";
            this.cmdClear.UseVisualStyleBackColor = true;
            // 
            // pTeam2
            // 
            this.pTeam2.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pTeam2.ErrorImage")));
            this.pTeam2.Location = new System.Drawing.Point(643, 42);
            this.pTeam2.Margin = new System.Windows.Forms.Padding(2);
            this.pTeam2.Name = "pTeam2";
            this.pTeam2.Size = new System.Drawing.Size(150, 100);
            this.pTeam2.TabIndex = 8;
            this.pTeam2.TabStop = false;
            // 
            // pTeam1
            // 
            this.pTeam1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pTeam1.ErrorImage")));
            this.pTeam1.Location = new System.Drawing.Point(34, 42);
            this.pTeam1.Margin = new System.Windows.Forms.Padding(2);
            this.pTeam1.Name = "pTeam1";
            this.pTeam1.Size = new System.Drawing.Size(150, 100);
            this.pTeam1.TabIndex = 7;
            this.pTeam1.TabStop = false;
            // 
            // cmbTeams
            // 
            this.cmbTeams.FormattingEnabled = true;
            this.cmbTeams.Location = new System.Drawing.Point(306, 83);
            this.cmbTeams.Margin = new System.Windows.Forms.Padding(2);
            this.cmbTeams.Name = "cmbTeams";
            this.cmbTeams.Size = new System.Drawing.Size(208, 21);
            this.cmbTeams.TabIndex = 1;
            this.cmbTeams.SelectedIndexChanged += new System.EventHandler(this.cmbTeams_SelectedIndexChanged);
            // 
            // oleDbSelectCommand1
            // 
            this.oleDbSelectCommand1.CommandText = "SELECT sName, sAbbrev, ID_Team FROM tblTeams";
            this.oleDbSelectCommand1.Connection = this.dbConnection;
            // 
            // dbConnection
            // 
            this.dbConnection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\nhl94.mdb";
            // 
            // oleDbInsertCommand1
            // 
            this.oleDbInsertCommand1.CommandText = "INSERT INTO `tblTeams` (`sName`, `sAbbrev`) VALUES (?, ?)";
            this.oleDbInsertCommand1.Connection = this.dbConnection;
            this.oleDbInsertCommand1.Parameters.AddRange(new System.Data.OleDb.OleDbParameter[] {
            new System.Data.OleDb.OleDbParameter("sName", System.Data.OleDb.OleDbType.VarWChar, 0, "sName"),
            new System.Data.OleDb.OleDbParameter("sAbbrev", System.Data.OleDb.OleDbType.VarWChar, 0, "sAbbrev")});
            // 
            // oleDbUpdateCommand1
            // 
            this.oleDbUpdateCommand1.CommandText = "UPDATE `tblTeams` SET `sName` = ?, `sAbbrev` = ? WHERE (((? = 1 AND `sName` IS NU" +
                "LL) OR (`sName` = ?)) AND ((? = 1 AND `sAbbrev` IS NULL) OR (`sAbbrev` = ?)) AND" +
                " (`ID_Team` = ?))";
            this.oleDbUpdateCommand1.Connection = this.dbConnection;
            this.oleDbUpdateCommand1.Parameters.AddRange(new System.Data.OleDb.OleDbParameter[] {
            new System.Data.OleDb.OleDbParameter("sName", System.Data.OleDb.OleDbType.VarWChar, 0, "sName"),
            new System.Data.OleDb.OleDbParameter("sAbbrev", System.Data.OleDb.OleDbType.VarWChar, 0, "sAbbrev"),
            new System.Data.OleDb.OleDbParameter("IsNull_sName", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "sName", System.Data.DataRowVersion.Original, true, null),
            new System.Data.OleDb.OleDbParameter("Original_sName", System.Data.OleDb.OleDbType.VarWChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "sName", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("IsNull_sAbbrev", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "sAbbrev", System.Data.DataRowVersion.Original, true, null),
            new System.Data.OleDb.OleDbParameter("Original_sAbbrev", System.Data.OleDb.OleDbType.VarWChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "sAbbrev", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_ID_Team", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ID_Team", System.Data.DataRowVersion.Original, null)});
            // 
            // oleDbDeleteCommand1
            // 
            this.oleDbDeleteCommand1.CommandText = "DELETE FROM `tblTeams` WHERE (((? = 1 AND `sName` IS NULL) OR (`sName` = ?)) AND " +
                "((? = 1 AND `sAbbrev` IS NULL) OR (`sAbbrev` = ?)) AND (`ID_Team` = ?))";
            this.oleDbDeleteCommand1.Connection = this.dbConnection;
            this.oleDbDeleteCommand1.Parameters.AddRange(new System.Data.OleDb.OleDbParameter[] {
            new System.Data.OleDb.OleDbParameter("IsNull_sName", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "sName", System.Data.DataRowVersion.Original, true, null),
            new System.Data.OleDb.OleDbParameter("Original_sName", System.Data.OleDb.OleDbType.VarWChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "sName", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("IsNull_sAbbrev", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "sAbbrev", System.Data.DataRowVersion.Original, true, null),
            new System.Data.OleDb.OleDbParameter("Original_sAbbrev", System.Data.OleDb.OleDbType.VarWChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "sAbbrev", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_ID_Team", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ID_Team", System.Data.DataRowVersion.Original, null)});
            // 
            // dbAdapter
            // 
            this.dbAdapter.DeleteCommand = this.oleDbDeleteCommand1;
            this.dbAdapter.InsertCommand = this.oleDbInsertCommand1;
            this.dbAdapter.SelectCommand = this.oleDbSelectCommand1;
            this.dbAdapter.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "tblTeams", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("sName", "sName"),
                        new System.Data.Common.DataColumnMapping("sAbbrev", "sAbbrev"),
                        new System.Data.Common.DataColumnMapping("ID_Team", "ID_Team")})});
            this.dbAdapter.UpdateCommand = this.oleDbUpdateCommand1;
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(709, 441);
            this.cmdClose.Margin = new System.Windows.Forms.Padding(2);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(88, 25);
            this.cmdClose.TabIndex = 10;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(4, 194);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(823, 167);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Coach Management";
            // 
            // settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(832, 485);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "settings";
            this.Text = "settings";
            this.Load += new System.EventHandler(this.settings_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pTeam2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTeam1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbTeams;
        private System.Data.OleDb.OleDbCommand oleDbSelectCommand1;
        private System.Data.OleDb.OleDbConnection dbConnection;
        private System.Data.OleDb.OleDbCommand oleDbInsertCommand1;
        private System.Data.OleDb.OleDbCommand oleDbUpdateCommand1;
        private System.Data.OleDb.OleDbCommand oleDbDeleteCommand1;
        private System.Data.OleDb.OleDbDataAdapter dbAdapter;
        private System.Windows.Forms.Button cmdClear;
        private System.Windows.Forms.PictureBox pTeam2;
        private System.Windows.Forms.PictureBox pTeam1;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}