using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StatExtractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Find the byte
            BinaryReader br = new BinaryReader(File.OpenRead("c:\\nhl94.gs0"));
            getTeamStats(br);
            getPlayerStats(br);
            br.Close();
        }

        private void getTeamStats(BinaryReader br)
        {
            
            // first val is address in decimal
            // second val is number of bytes to read
            // third is the binaryreader object passed to this method to access the file
            GlobalVar.iHteam = getValue(59305,1,br);                         //home team
            GlobalVar.iHgoals = getValue(60242, 2, br);                        //home goals
            GlobalVar.iHshots = getValue(60230, 2, br);                      //home shots
            GlobalVar.iHPPgoals = getValue(60232, 2, br);                    //home PP goals
            GlobalVar.iHPPattempts = getValue(60234, 2, br);                 //home PP attempts
            GlobalVar.iHSHgoals = getValue(61084, 2, br);                    //home SH goals
            GlobalVar.iHbreaks = getValue(61086, 2, br);                     //home breakaways
            GlobalVar.iHbreakGoals = getValue(61088, 2, br);                 //home breakaway goals
            GlobalVar.iHoneT = getValue(61090, 2, br);                       //home one-timers
            GlobalVar.iHoneTgoals = getValue(61092, 2, br);                  //home one-timers
            GlobalVar.iHchecks = getValue(60246, 2, br);                     //home checks
            GlobalVar.iHpenalties = getValue(60236, 2, br);                  //home penalties

            GlobalVar.iAteam = getValue(59307, 1, br);                       //away team
            GlobalVar.iAgoals = getValue(61110, 2, br);                      //away goals
            GlobalVar.iAshots = getValue(61098, 2, br);                      //away shots
            GlobalVar.iAPPgoals = getValue(61100, 2, br);                    //away PP goals
            GlobalVar.iAPPattempts = getValue(61102, 2, br);                 //away PP attempts
            GlobalVar.iASHgoals = getValue(61952, 2, br);                    //away SH goals
            GlobalVar.iAbreaks = getValue(61954, 2, br);                     //away breakaways
            GlobalVar.iAbreakGoals = getValue(61956, 2, br);                 //away breakaway goals
            GlobalVar.iAoneT = getValue(61958, 2, br);                       //away one-timers
            GlobalVar.iAoneTgoals = getValue(61960, 2, br);                  //away one-timers
            GlobalVar.iAchecks = getValue(61114, 2, br);                     //away checks
            GlobalVar.iApenalties = getValue(61104, 2, br);                  //away penalties
        }

        private void getPlayerStats(BinaryReader br)
        {

            // first val is address in decimal
            // second val is number of bytes to read
            // third is the binaryreader object passed to this method to access the file
            GlobalVar.iHPgoals = getValue(60242, 2, br);                      //home goals
            GlobalVar.iHPshots = getValue(60230, 2, br);                      //home shots
            GlobalVar.iHPassists = getValue(60232, 2, br);                    //home assists

            GlobalVar.iAPgoals = getValue(61110, 2, br);                      //away goals
            GlobalVar.iAPshots = getValue(61098, 2, br);                      //away shots
            GlobalVar.iAPassists = getValue(61100, 2, br);                    //away assists
        }

        private int getValue(int iAddress, int iBytes, BinaryReader br)
        {
            br.BaseStream.Position = 0;
            br.BaseStream.Position = iAddress;
            byte[] bData = br.ReadBytes(iBytes);
            int intVal = 0;
            for (int i = 0; i < bData.Length; i++)
            {
                intVal = intVal + (int)bData[i];
            }

            return intVal;
        }
    }

    public static class GlobalVar
    {
            public static int iHteam = 0;
            public static int iHgoals = 0;
            public static int iHshots = 0;
            public static int iHPPgoals = 0;
            public static int iHPPattempts = 0;
            public static int iHSHgoals = 0;
            public static int iHbreaks = 0;
            public static int iHbreakGoals = 0;
            public static int iHoneT = 0;
            public static int iHoneTgoals = 0;
            public static int iHchecks = 0;
            public static int iHpenalties = 0;

            public static int iAteam = 0;
            public static int iAgoals = 0;
            public static int iAshots = 0;
            public static int iAPPgoals = 0;
            public static int iAPPattempts = 0;
            public static int iASHgoals = 0;
            public static int iAbreaks = 0;
            public static int iAbreakGoals = 0;
            public static int iAoneT = 0;
            public static int iAoneTgoals = 0;
            public static int iAchecks = 0;
            public static int iApenalties = 0;

            public static int iHPgoals = 0;
            public static int iHPshots = 0;
            public static int iHPassists = 0;

            public static int iAPgoals = 0;
            public static int iAPshots = 0;
            public static int iAPassists = 0;
    }

}