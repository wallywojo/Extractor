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
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void cmdSettings_Click(object sender, EventArgs e)
        {
            settings dlgSettings = new settings();
            dlgSettings.Tag = this;
            dlgSettings.Show(this);
            Hide();
        }

        private void cmdExtract_Click(object sender, EventArgs e)
        {
            Extractor dlg = new Extractor();
            dlg.Tag = this;
            dlg.Show(this);
            Hide();
        }

    }
}
