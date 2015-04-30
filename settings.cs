using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StatExtractor
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
        }

        private void settings_Load(object sender, EventArgs e)
        {
            // choose table to query against, using design time statement with parameter
            // return from query stored in this object
            nhl94DataSet.tblTeamsDataTable tbl = new nhl94DataSet.tblTeamsDataTable();
            // get adapter class object to run query
            nhl94DataSetTableAdapters.tblTeamsTableAdapter tAdapter = new nhl94DataSetTableAdapters.tblTeamsTableAdapter();
            // first param is object to store query results
            // second is what to search for in predefined statement
            tAdapter.FillJustTeamAbbrev(tbl);

            // can use object.rows as an array
            if (tbl.Rows.Count > 0)
            {
                cmbTeams.Items.Add("");
                cmbTeams.DataSource = tbl;
                cmbTeams.DisplayMember = "sName";
                cmbTeams.ValueMember = "sAbbrev";
            }
        }

        private void cmbTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            pTeam1.ImageLocation = "assets\\" + cmbTeams.SelectedValue.ToString() + ".gif";
            pTeam2.ImageLocation = "assets\\" + cmbTeams.SelectedValue.ToString() + ".gif";
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            var dlgMain = (MainPage)Tag;
            dlgMain.Show();
            Close();
        }
    }
}
