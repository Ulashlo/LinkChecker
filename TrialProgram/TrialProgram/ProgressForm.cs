using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrialProgram
{
    public partial class ProgressForm : Form
    {
        PageLinksSQL links;
        Action visualise, update;

        public ProgressForm(PageLinksSQL l, Action visualise, Action update)
        {
            InitializeComponent();
            this.links = l;
            this.visualise = visualise;
            this.update = update;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = false;
        }

        private void ProgressForm_Shown(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            timer1.Start();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            update();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Count.Text = links.Count.ToString();
            CountIner.Text = links.InnerCount.ToString();
            CountOuter.Text = links.OuterCount.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            visualise();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            backgroundWorker1.ReportProgress(1);
        }
    }
}
