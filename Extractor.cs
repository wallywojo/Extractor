using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StatExtractor
{
    public partial class Extractor : Form
    {
        public Extractor()
        {
            InitializeComponent();

        }

        private void cmdPreview_Click(object sender, EventArgs e)
        {
            string szPyScript;
            if (rTeam28.Checked == true)
                szPyScript = "Gens_Stat_Extractor_Orig.py";
            else
                szPyScript = "Gens_Stat_Extractor.py";

            ProcessStartInfo myInfo = new ProcessStartInfo();

            myInfo.FileName = Environment.GetEnvironmentVariable("PYTHON_PATH")  + "\\python.exe";
            myInfo.Arguments = szPyScript.ToString() + " -s " + GlobalVar.sSave.ToString() + " -r " + GlobalVar.sRom.ToString();
            myInfo.UseShellExecute = false;
            myInfo.CreateNoWindow = true;
            
            try
            {
                using (Process exeProcess = Process.Start(myInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running python script" + ex.ToString());
            }

            StreamReader sr = new StreamReader(File.OpenRead("Gens_SavestateData.csv"));
            List<string> lStats = new List<string>();

            while (!sr.EndOfStream)
            {
                lStats.Add(sr.ReadLine());
            }

            // finished importing stats
            // preview stats in UI
            getTeamStats(lStats);
            getPeriodStats(lStats);
            getGoalSummary(lStats);
            getPenaltySummary(lStats);
            getPlayerStats(lStats);

            // #### get the coaches
            // choose table to query against, using design time statement with parameter
            // return from query stored in this object
            nhl94DataSet.tblCoachesDataTable tbl = new nhl94DataSet.tblCoachesDataTable();
            // get adapter class object to run query
            nhl94DataSetTableAdapters.tblCoachesTableAdapter tAdapter = new nhl94DataSetTableAdapters.tblCoachesTableAdapter();
            // first param is object to store query results
            // second is what to search for in predefined statement
            tAdapter.FillMinimal(tbl);

            // can use object.rows as an array
            if (tbl.Rows.Count > 0)
            {
                // add a blank to start
                cmbAway.Items.Add("");
                cmbHome.Items.Add("");
                cmbAway.DataSource = tbl;
                cmbAway.DisplayMember = "sName";
                cmbAway.ValueMember = "sAbbrev";
                cmbHome.DataSource = tbl;
                cmbHome.DisplayMember = "sName";
                cmbHome.ValueMember = "sAbbrev";
            }
        }

        private void getTeamStats(List<string> lStats)
        {
            // all stats up until first blank line are team stats
            string[] sTemp;
            double dUtil = 0;

            //first is team name
            sTemp = lStats[0].Split(',');
            lblAwayName.Text = sTemp[1];
            lblHomeName.Text = sTemp[2];

            //team logo
            pAway.ImageLocation = "assets\\" + sTemp[1] + ".gif";
            pHome.ImageLocation = "assets\\" + sTemp[2] + ".gif";

            //goals
            sTemp = lStats[1].Split(',');
            lblAwayScore.Text = sTemp[1];
            lblA_Goals.Text = sTemp[1];
            lblA_F.Text = sTemp[1];
            lblHomeScore.Text = sTemp[2];
            lblH_Goals.Text = sTemp[2];
            lblH_F.Text = sTemp[2];

            //shots
            sTemp = lStats[2].Split(',');
            lblA_Shots.Text = sTemp[1];
            lblH_Shots.Text = sTemp[2];

            //shoot P
            sTemp = lStats[3].Split(',');
            dUtil = Convert.ToDouble(sTemp[1]) * 100;
            lblA_ShootP.Text = String.Format("{0}%",Convert.ToInt32(dUtil));
            dUtil = Convert.ToDouble(sTemp[2]) * 100;
            lblH_ShootP.Text = String.Format("{0}%", Convert.ToInt32(dUtil));

            //PPG
            sTemp = lStats[4].Split(',');
            lblA_PPG.Text = sTemp[1];
            lblH_PPG.Text = sTemp[2];

            //PPA
            sTemp = lStats[5].Split(',');
            lblA_PPG.Text = lblA_PPG.Text + " / " + sTemp[1];
            lblH_PPG.Text = lblH_PPG.Text + " / " + sTemp[2];

            //PPMin
            sTemp = lStats[7].Split(',');
            lblA_PPMin.Text = sTemp[1];
            lblH_PPMin.Text = sTemp[2];

            //SH goals
            sTemp = lStats[27].Split(',');
            lblA_SH.Text = sTemp[1];
            lblH_SH.Text = sTemp[2];

            //Breakaways
            sTemp = lStats[28].Split(',');
            lblA_Break.Text = sTemp[1];
            lblH_Break.Text = sTemp[2];

            //Breakaway Goals
            sTemp = lStats[29].Split(',');
            lblA_Break.Text = lblA_Break.Text + " / " + sTemp[1];
            lblH_Break.Text = lblH_Break.Text + " / " + sTemp[2];

            //1t
            sTemp = lStats[31].Split(',');
            lblA_1T.Text = sTemp[1];
            lblH_1T.Text = sTemp[2];

            //1t Goals
            sTemp = lStats[32].Split(',');
            lblA_1T.Text = lblA_1T.Text + " / " + sTemp[1];
            lblH_1T.Text = lblH_1T.Text + " / " + sTemp[2];

            //Faceoff P
            sTemp = lStats[11].Split(',');
            dUtil = Convert.ToDouble(sTemp[1]) * 100;
            lblA_FaceP.Text = String.Format("{0}%", Convert.ToInt32(dUtil));
            dUtil = Convert.ToDouble(sTemp[2]) * 100;
            lblH_FaceP.Text = String.Format("{0}%", Convert.ToInt32(dUtil));

            //pass
            sTemp = lStats[13].Split(',');
            lblA_Pass.Text = sTemp[1];
            lblH_Pass.Text = sTemp[2];

            //pass att
            sTemp = lStats[14].Split(',');
            lblA_Pass.Text = lblA_Pass.Text + " / " + sTemp[1];
            lblH_Pass.Text = lblH_Pass.Text + " / " + sTemp[2];

            //Pass P
            sTemp = lStats[15].Split(',');
            dUtil = Convert.ToDouble(sTemp[1]) * 100;
            lblA_PassP.Text = String.Format("{0}%", Convert.ToInt32(dUtil));
            dUtil = Convert.ToDouble(sTemp[2]) * 100;
            lblH_PassP.Text = String.Format("{0}%", Convert.ToInt32(dUtil));

        }

        private void getPeriodStats(List<string> lStats)
        {
            // all stats up until first blank line are team stats
            string[] sTemp;

            //first
            sTemp = lStats[16].Split(',');
            lblA_1st.Text = sTemp[1];
            lblH_1st.Text = sTemp[2];

            //second
            sTemp = lStats[17].Split(',');
            lblA_2nd.Text = sTemp[1];
            lblH_2nd.Text = sTemp[2];

            //third
            sTemp = lStats[18].Split(',');
            lblA_3rd.Text = sTemp[1];
            lblH_3rd.Text = sTemp[2];

            //OT
            sTemp = lStats[19].Split(',');
            lblA_OT.Text = sTemp[1];
            lblH_OT.Text = sTemp[2];
        }

        private void getGoalSummary(List<string> lStats)
        {

            // create list for datagrid, to be converted to array
            List<string> lGoals = new List<string>();

            // first data after blank lines is start away scoring
            string[] sTemp, sTime;
            string sUtil = "";
            int iSeconds, i = 0;

            //first is team name, use for indicating who scored
            sTemp = lStats[0].Split(',');
            string sAway = sTemp[1];
            string sHome = sTemp[2];

            //start of scoring
            i = 41;

            //iterate through home goals
            while (lStats[i].ToString() != "")
            {
                sTemp = lStats[i].Split(',');
                sUtil = sTemp[1];  // time in minutes
                sTime = sUtil.Split(':');
                iSeconds = (Convert.ToInt32(sTime[0]) * 60) + Convert.ToInt32(sTime[1]);
                switch (sTemp[0])
                {
                    case "2nd":
                        iSeconds = iSeconds + 300;
                        break;
                    case "3rd":
                        iSeconds = iSeconds + 600;
                        break;
                    case "OT":
                        iSeconds = iSeconds + 900;
                        break;
                }
                lGoals.Add(sHome.ToString() + "," + lStats[i].ToString() + "," + iSeconds);
                i++;
            }

            // skip blank data
            i=i+3;

            //iterate through away goals
            while (lStats[i].ToString() != "")
            {
                sTemp = lStats[i].Split(',');
                sUtil = sTemp[1];  // time in minutes
                sTime = sUtil.Split(':');
                iSeconds = (Convert.ToInt32(sTime[0]) * 60) + Convert.ToInt32(sTime[1]);
                switch (sTemp[0])
                {
                    case "2nd":
                        iSeconds = iSeconds + 300;
                        break;
                    case "3rd":
                        iSeconds = iSeconds + 600;
                        break;
                    case "OT":
                        iSeconds = iSeconds + 900;
                        break;
                }
                lGoals.Add(sAway.ToString() + "," + lStats[i].ToString() + "," + iSeconds);
                i++;
            }

            DataGridViewTextBoxColumn Team = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Period = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Time = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Scorer = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Assist1 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Assist2 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn TeamStrength = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn TotalTime = new DataGridViewTextBoxColumn();

            Team.HeaderText = "Team";
            Period.HeaderText = "Period";
            Time.HeaderText = "Time";
            Scorer.HeaderText = "Scorer";
            Assist1.HeaderText = "Assist 1";
            Assist2.HeaderText = "Assist 2";
            TeamStrength.HeaderText = "Team Strength";

            dgScoring.Columns.Add(Team);
            dgScoring.Columns.Add(Period);
            dgScoring.Columns.Add(Time);
            dgScoring.Columns.Add(Scorer);
            dgScoring.Columns.Add(Assist1);
            dgScoring.Columns.Add(Assist2);
            dgScoring.Columns.Add(TeamStrength);
            dgScoring.Columns.Add(TotalTime);
            for (int iGoals=0; iGoals < lGoals.Count; iGoals++)
            {
                dgScoring.Rows.Add(lGoals[iGoals].Split(','));
            }

            dgScoring.Columns[7].Visible = false;
            dgScoring.Sort(TotalTime, ListSortDirection.Ascending);
        }

        private void getPenaltySummary(List<string> lStats)
        {

            // create list for datagrid, to be converted to array
            List<string> lPenalties = new List<string>();

            // first data after blank lines is start away scoring
            string[] sTemp,sTime;
            string sUtil = "";
            int iSeconds,i = 0;

            //first is team name, use for indicating who scored
            sTemp = lStats[0].Split(',');
            string sAway = sTemp[1];
            string sHome = sTemp[2];

            //start of penalties

            i = 49;

            //iterate through home penalties
            while (lStats[i].ToString() != "")
            {
                sTemp = lStats[i].Split(',');
                sUtil = sTemp[1];  // time in minutes
                sTime = sUtil.Split(':');
                iSeconds = (Convert.ToInt32(sTime[0]) * 60) + Convert.ToInt32(sTime[1]);
                switch (sTemp[0])
                {
                    case "2nd":
                        iSeconds = iSeconds + 300;
                        break;
                    case "3rd":
                        iSeconds = iSeconds + 600;
                        break;
                    case "OT":
                        iSeconds = iSeconds + 900;
                        break;
                }

                lPenalties.Add(sHome.ToString() + "," + lStats[i].ToString() + "," + iSeconds);
                i++;
            }

            // skip blank data
            i = i + 3;

            //iterate through away penalties
            while (lStats[i].ToString() != "")
            {
                sTemp = lStats[i].Split(',');
                sUtil = sTemp[1];  // time in minutes
                sTime = sUtil.Split(':');
                iSeconds = (Convert.ToInt32(sTime[0]) * 60) + Convert.ToInt32(sTime[1]);
                switch (sTemp[0])
                {
                    case "2nd":
                        iSeconds = iSeconds + 300;
                        break;
                    case "3rd":
                        iSeconds = iSeconds + 600;
                        break;
                    case "OT":
                        iSeconds = iSeconds + 900;
                        break;
                }
                lPenalties.Add(sAway.ToString() + "," + lStats[i].ToString() + "," + iSeconds);
                i++;
            }

            DataGridViewTextBoxColumn Team = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Period = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Time = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn cTime = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Player = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Penalty = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn TotalTime = new DataGridViewTextBoxColumn();

            Team.HeaderText = "Team";
            Period.HeaderText = "Period";
            Time.HeaderText = "Time";
            Player.HeaderText = "Player";
            Penalty.HeaderText = "Penalty";

            dgPenalty.Columns.Add(Team);
            dgPenalty.Columns.Add(Period);
            dgPenalty.Columns.Add(Time);
            dgPenalty.Columns.Add(Player);
            dgPenalty.Columns.Add(Penalty);
            dgPenalty.Columns.Add(TotalTime);
            for (int iGoals = 0; iGoals < lPenalties.Count; iGoals++)
            {
                dgPenalty.Rows.Add(lPenalties[iGoals].Split(','));
            }

            dgPenalty.Columns[5].Visible = false;
            dgPenalty.Sort(TotalTime, ListSortDirection.Ascending);
        }

        private void getPlayerStats(List<string> lStats)
        {
            // create list for datagrid
            List<string> hPlayers = new List<string>();
            List<string> aPlayers = new List<string>();

            // first data after blank lines is start away scoring
            string[] sTemp;
            int i = 0;

            //start of scoring
            i = 66;

            //iterate through home players

            //Forwards
            while (lStats[i].ToString() != "")
            {
                sTemp = lStats[i].Split(',');
                hPlayers.Add(sTemp[0] + "," + sTemp[1] + "," + sTemp[2] + "," + sTemp[3] + "," + sTemp[4]);
                i++;
            }

            // skip blank data
            i = i + 3;

            //Defensemen
            while (lStats[i].ToString() != "")
            {
                sTemp = lStats[i].Split(',');
                hPlayers.Add(sTemp[0] + "," + sTemp[1] + "," + sTemp[2] + "," + sTemp[3] + "," + sTemp[4]);
                i++;
            }

            //skip over blank data/goalies
            i = i + 13;

            //iterate through away players

            //Forwards
            while (lStats[i].ToString() != "")
            {
                sTemp = lStats[i].Split(',');
                aPlayers.Add(sTemp[0] + "," + sTemp[1] + "," + sTemp[2] + "," + sTemp[3] + "," + sTemp[4]);
                i++;
            }

            // skip blank data
            i = i + 3;

            //Defensemen
            while (i < lStats.Count)
            {
                sTemp = lStats[i].Split(',');
                aPlayers.Add(sTemp[0] + "," + sTemp[1] + "," + sTemp[2] + "," + sTemp[3] + "," + sTemp[4]);
                i++;
            }

            DataGridViewTextBoxColumn aPlayer = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn aGoals = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn aAssists = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn aPoints = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn aSOG = new DataGridViewTextBoxColumn();

            DataGridViewTextBoxColumn hPlayer = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn hGoals = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn hAssists = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn hPoints = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn hSOG = new DataGridViewTextBoxColumn();

            aPlayer.HeaderText = "Player";
            aGoals.HeaderText = "G";
            aAssists.HeaderText = "A";
            aPoints.HeaderText = "P";
            aSOG.HeaderText = "SOG";

            hPlayer.HeaderText = "Player";
            hGoals.HeaderText = "G";
            hAssists.HeaderText = "A";
            hPoints.HeaderText = "P";
            hSOG.HeaderText = "SOG";

            dgAwayPlayers.Columns.Add(aPlayer);
            dgAwayPlayers.Columns.Add(aGoals);
            dgAwayPlayers.Columns.Add(aAssists);
            dgAwayPlayers.Columns.Add(aPoints);
            dgAwayPlayers.Columns.Add(aSOG);

            dgHomePlayers.Columns.Add(hPlayer);
            dgHomePlayers.Columns.Add(hGoals);
            dgHomePlayers.Columns.Add(hAssists);
            dgHomePlayers.Columns.Add(hPoints);
            dgHomePlayers.Columns.Add(hSOG);

            for (int iGoals = 0; iGoals < aPlayers.Count; iGoals++)
            {
                dgAwayPlayers.Rows.Add(aPlayers[iGoals].Split(','));
            }

            for (int iGoals = 0; iGoals < hPlayers.Count; iGoals++)
            {
                dgHomePlayers.Rows.Add(hPlayers[iGoals].Split(','));
            }

            dgAwayPlayers.Sort(aPoints, ListSortDirection.Descending);
            dgHomePlayers.Sort(hPoints, ListSortDirection.Descending);
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string[] temp;
            dlgOpen.ShowDialog();
            GlobalVar.sSave = dlgOpen.FileName;
            temp = dlgOpen.FileName.Split('\\');
            int iTemp = temp.Length;
            lblSave.Text = temp[iTemp - 1];
        }

        private void cmdROM_Click(object sender, EventArgs e)
        {
            string[] temp;
            dlgOpen.ShowDialog();
            GlobalVar.sRom = dlgOpen.FileName;
            temp = dlgOpen.FileName.Split('\\');
            int iTemp = temp.Length;
            lblROM.Text = temp[iTemp - 1];
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            var dlgMain = (MainPage)Tag;
            dlgMain.Show();
            Close();
        }

        private void cmdImport_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(File.OpenRead("Gens_SavestateData.csv"));
            List<string> lStats = new List<string>();

            while (!sr.EndOfStream)
            {
                lStats.Add(sr.ReadLine());
            }

            // finished importing stats
            //first is team name
            string[] sTemp;
            sTemp = lStats[0].Split(',');
            lblAwayName.Text = sTemp[1];
            lblHomeName.Text = sTemp[2];

            try
            {
                // choose table to query against, using design time statement with parameter
                // return from query stored in this object
                nhl94DataSet.tblTeamsDataTable tbl = new nhl94DataSet.tblTeamsDataTable();
                // get adapter class object to run query
                nhl94DataSetTableAdapters.tblTeamsTableAdapter tAdapter = new nhl94DataSetTableAdapters.tblTeamsTableAdapter();
                // first param is object to store query results
                // second is what to search for in predefined statement
                tAdapter.FillByAbbrev(tbl, sTemp[1]);

                // can use object.rows as an array
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow row in tbl.Rows)
                    {
                        MessageBox.Show("The full name is " + row[1] + " " + row[3]);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }

    public static class GlobalVar
    {

            public static string sSave = "";
            public static string sRom = "";
    }

}