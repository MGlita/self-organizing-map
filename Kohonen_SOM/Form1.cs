using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kohonen_SOM
{
    public partial class Form1 : Form
    {
        List<PictureBox> lst = new List<PictureBox>();
        Map mp=new Map(7,3);
        int epochs;
        double[][] data = new double[6][] {   new double[] {1,0,0},
                                              new double[] {0,1,0},
                                              new double[] {0,0,1},
                                              new double[] {1,1,0},
                                              new double[] {0,1,1},
                                              new double[] {1,0,1}
                                             };
        public Form1()
        {
            InitializeComponent();
            AddToList();
        }
                      
        private async void Start_Click(object sender, EventArgs e)
        {
            epochs = Convert.ToInt32(epochsNumBox.Value);
            List<string> list = new List<string>();
            for (int i = 0; i < epochs; i++)
                list.Add(i.ToString());
            lblStatus.Text = "Working...";
            var progress = new Progress<ProgressReport>();
            progress.ProgressChanged += (o, report) =>
            {
                lblStatus.Text = string.Format("Processing...{0}%", report.PercentComplete);
                progressBar1.Value = report.PercentComplete;
                progressBar1.Update();
            };
            await PerformTraining(list, progress);
            lblStatus.Text = "Done!";
        }

        private void InitColors_Click(object sender, EventArgs e)
        {
            ShowColors();
        }

        private void Restartbtn_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void progressBar1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = Math.Min(e.ProgressPercentage, 100);
        }

        void ShowColors()
        {
            int k = 0;
            for (int i = 0; i < mp.Nodes.GetLength(1); i++)
            {
                for (int j = 0; j < mp.Nodes.GetLength(1); j++)
                {
                    lst[k].BackColor = Color.FromArgb(Convert.ToInt32(mp.Nodes[i, j].weights[0] * 255), Convert.ToInt32(mp.Nodes[i, j].weights[1] * 255), Convert.ToInt32(mp.Nodes[i, j].weights[2] * 255));
                    k++;
                }
            }
        }

        void AddToList()
        {
            for (int i = 1; i <= 49; i++)
            {
               PictureBox tbx = this.Controls.Find("pictureBox" + i.ToString(), true).FirstOrDefault() as PictureBox;
                lst.Add(tbx);
            }
   
        }
      
        private Task PerformTraining(List<string> list, IProgress<ProgressReport> progress)
        {
            int index = 1;
            int totalProcess = list.Count;
            var progressReport = new ProgressReport();
            return Task.Run(() =>
            {
                for (int i = 0; i < totalProcess; i++)
                {                  
                    mp.Train(7, epochs, data);
                    ShowColors();                   
                    Application.DoEvents();
                    progressReport.PercentComplete = index++ * 100 / totalProcess;
                    progress.Report(progressReport);
                }
            });
        }        
    }
}
