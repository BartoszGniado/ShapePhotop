using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapePhoto
{
    
    public partial class Form1 : Form
    {
        Random rnd;
        int AproxNum;
        int ShapeNum;
        int ShapeNumMin;
        List<Aprox> Aproxes;
        //  int lowestApxScoreID;
        int[] LOCALlowestApxScoreID;
        int LOCALlowestApxScoreID2;

        public Form1()
        {
            InitializeComponent();
            AproxNum = 10;
            ShapeNum = 10;
            Aproxes = new List<Aprox>();
            AproxesCopy = new List<Aprox>();
            LOCALlowestApxScoreID = new int[2];
            totalIt = 0;
        }
        
        private void fileSelect_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                pictureBoxOrginal.Image = Image.FromFile(textBox1.Text);
                button2.Enabled = true;
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            pictureBoxOrginal.Image = Image.FromFile(textBox1.Text);
            Aproxes.Clear();
            flowLayoutPanel1.Controls.Clear();
                

            Scores.Items.Clear();
            
            int i;
            rnd = new Random();
            ShapeNum = (int)numericUpDown1.Value;
            ShapeNumMin = (int)numericUpDown4.Value;
            AproxNum = (int)numericUpDown3.Value;
            //   Aproxes.Add(new Aprox( pictureBoxOrginal.Image));
            for (i = 0; i < AproxNum; i++)
                Aproxes.Add(new Aprox(rnd.Next(ShapeNum)+ ShapeNumMin, pictureBoxOrginal.Image,rnd.Next(), i));
           // SetLowApxScrID();
            for (i=0;i< Aproxes.Count;i++)
            {
                AddApxToSelection(i, Aproxes[i]);
                Scores.Items.Add(Aproxes[i].score);
            }
      //      Aprox a = new Aprox(ShapeNum, pictureBoxOrginal.Image);
     //       pictureBoxSelected.Image = a.madeImage;
        //    button2.Enabled = false;
            tabControl1.SelectedIndex = 2;
        }

        List<Aprox> AproxesCopy;
        int totalIt;
        private void Iterations_Click(object sender, EventArgs e)
        {
            
            int num =(int) numericUpDown2.Value;
            int num2 = num;
            int numCopy;
            Scores.SelectedIndexChanged -= Scores_SelectedIndexChanged;
            int podzialID = 1 + rnd.Next(Aproxes.Count - 2);
            AproxesCopy.Clear();
            foreach (Aprox a in Aproxes)
                AproxesCopy.Add(a);// new Aprox(a));
            SetLocalLowApxScrID( podzialID,  0);
            SetLocalLowApxScrID(podzialID, 1);
            int podzialIDCopy = podzialID;
            Image orginl =(Image) pictureBoxOrginal.Image.Clone();
            Random rndCopy = new Random();
            numCopy = num;
            int ShapeNumCopy = ShapeNum;
            int ShapeNumMinCopy = ShapeNumMin;

            Thread t1 = MakeThread(AproxesCopy, orginl, numCopy, ShapeNumCopy, ShapeNumMinCopy, rndCopy, podzialIDCopy);


            //   t1.Start();
            num2--;
            for (int i=0;i<num;i++,num2--)//, numericUpDown2.Value--)
            {
                if (num2 % 100 == 0)
                {
                    label3.Text = num2.ToString();
                    label3.Update();
                }
                
                
                Aprox apx;
                if (rnd.NextDouble() > 0.15)
                {
                    int ID = podzialID + rnd.Next(Aproxes.Count - podzialID);
                    apx = new Aprox(Aproxes[ID], (Image)pictureBoxOrginal.Image.Clone(), rnd.Next(), ID);
                    bool ok = true;
                    for (int idx = podzialID; idx < Aproxes.Count; idx++)
                        if (Aproxes[idx].score == apx.score)
                            ok = false;
                    if (apx.score < Aproxes[ID].score && ok)
                    {
                        if(ShowCheckBox.Checked)   flowLayoutPanel1.SuspendLayout();
                        if (ShowCheckBox.Checked) flowLayoutPanel1.Controls.RemoveAt(LOCALlowestApxScoreID2);
                        //      Scores.Items.RemoveAt(LOCALlowestApxScoreID2);
                        Aproxes.RemoveAt(LOCALlowestApxScoreID2);
                        Aproxes.Insert(LOCALlowestApxScoreID2, apx);
                        if (ShowCheckBox.Checked) AddApxToSelection(LOCALlowestApxScoreID2, apx);
                        SetLocalLowApxScrID(podzialID, 1);
                        if (ShowCheckBox.Checked) flowLayoutPanel1.Update();
                        //       Scores.Update();
                        if (ShowCheckBox.Checked) flowLayoutPanel1.ResumeLayout();
                    }
                }
                else
                {
                    apx = new Aprox(rnd.Next(ShapeNum) + ShapeNumMin, (Image)pictureBoxOrginal.Image.Clone(), rnd.Next(), -1);
                    bool ok = true;
                    for (int idx = podzialID; idx < Aproxes.Count; idx++)
                        if (Aproxes[idx].score == apx.score)
                            ok = false;
                    if (apx.score < Aproxes[LOCALlowestApxScoreID2].score && ok)
                    {
                        if (ShowCheckBox.Checked) flowLayoutPanel1.SuspendLayout();
                        if (ShowCheckBox.Checked) flowLayoutPanel1.Controls.RemoveAt(LOCALlowestApxScoreID2);
                        //      Scores.Items.RemoveAt(LOCALlowestApxScoreID2);
                        Aproxes.RemoveAt(LOCALlowestApxScoreID2);
                        Aproxes.Insert(LOCALlowestApxScoreID2, apx);
                        if (ShowCheckBox.Checked) AddApxToSelection(LOCALlowestApxScoreID2, apx);
                        SetLocalLowApxScrID(podzialID, 1);
                        if (ShowCheckBox.Checked) flowLayoutPanel1.Update();
                        //       Scores.Update();
                        if (ShowCheckBox.Checked) flowLayoutPanel1.ResumeLayout();
                    }
                }
                
            }
          //  t1.Join();
            for (int i = 0; i < podzialID; i++)
            {
                Aproxes[i] = AproxesCopy[i];
                flowLayoutPanel1.Controls.RemoveAt(i);
         //       Scores.Items.RemoveAt(i);
                AddApxToSelection(i, Aproxes[i]);
            }
            if (!ShowCheckBox.Checked)
                for (int i = podzialID; i < Aproxes.Count; i++)
                {
                    flowLayoutPanel1.Controls.RemoveAt(i);
                    //       Scores.Items.RemoveAt(i);
                    AddApxToSelection(i, Aproxes[i]);
                }
            Scores.Items.Clear();
            for (int i = 0; i < Aproxes.Count; i++)
            {
                Scores.Items.Add(Aproxes[i].score);
            }
            Scores.SelectedIndexChanged += Scores_SelectedIndexChanged;
            totalIt += num;
            label6.Text = totalIt.ToString();
        }

        private Thread MakeThread(List<Aprox> AproxesCopy,Image orginl,int numCopy,int ShapeNumCopy,int ShapeNumMinCopy,Random rndCopy,int podzialIDCopy)
        {
            Thread t1 = new Thread(() =>
            {
                for (int i = 0; i < numCopy; i++)//, numericUpDown2.Value--)
                {

                    Aprox apx;
                    if (rndCopy.NextDouble() > 0.15)
                    {
                        int ID = rndCopy.Next(podzialIDCopy);
                        apx = new Aprox(AproxesCopy[ID], (Image)orginl.Clone(), rndCopy.Next(), ID);
                        bool ok = true;
                        for (int idx = 0; idx < podzialIDCopy; idx++)
                            if (AproxesCopy[idx].score == apx.score)
                                ok = false;
                        if (apx.score < AproxesCopy[ID].score && ok)
                        {
                            //flowLayoutPanel1.SuspendLayout();    //
                            //flowLayoutPanel1.Controls.RemoveAt(LOCALlowestApxScoreID[0]); //
                            //Scores.Items.RemoveAt(LOCALlowestApxScoreID[0]);   //
                            AproxesCopy.RemoveAt(LOCALlowestApxScoreID[0]);
                            AproxesCopy.Insert(LOCALlowestApxScoreID[0], apx);
                            //AddApxToSelection(LOCALlowestApxScoreID[0], apx); //
                            SetLocalLowApxScrID(podzialIDCopy, 0);
                            //flowLayoutPanel1.Update();    //
                            //Scores.Update();  //
                            //flowLayoutPanel1.ResumeLayout();  //
                        }
                    }
                    else
                    {
                        apx = new Aprox(rndCopy.Next(ShapeNumCopy) + ShapeNumMinCopy, (Image)orginl.Clone(), rndCopy.Next(), -1);
                        bool ok = true;
                        for (int idx = 0; idx < podzialIDCopy; idx++)
                            if (AproxesCopy[idx].score == apx.score)
                                ok = false;
                        if (apx.score < AproxesCopy[LOCALlowestApxScoreID[0]].score && ok)
                        {
                            //flowLayoutPanel1.SuspendLayout();    //
                            //flowLayoutPanel1.Controls.RemoveAt(LOCALlowestApxScoreID[0]); //
                            //Scores.Items.RemoveAt(LOCALlowestApxScoreID[0]);   //
                            AproxesCopy.RemoveAt(LOCALlowestApxScoreID[0]);
                            AproxesCopy.Insert(LOCALlowestApxScoreID[0], apx);
                            //AddApxToSelection(LOCALlowestApxScoreID[0], apx); //
                            SetLocalLowApxScrID2(podzialIDCopy, 0);
                            //flowLayoutPanel1.Update();    //
                            //Scores.Update();  //
                            //flowLayoutPanel1.ResumeLayout();  //
                        }
                    }


                }
            });
            return t1;
        }

        private void AddApxToSelection(int id, Aprox apx)
        {
            PictureBox picBox = new PictureBox();
            picBox.SizeMode = PictureBoxSizeMode.Zoom;
            int SelectViewIconsSize = 222;
            picBox.Width = SelectViewIconsSize;
            picBox.Height = SelectViewIconsSize;
            picBox.Image = apx.madeImage;
            picBox.Click += new System.EventHandler(this.pictureBoxChoosen_Click);
            flowLayoutPanel1.Controls.Add(picBox);
            flowLayoutPanel1.Controls.SetChildIndex(picBox, id);
            //       Scores.Items.Add(apx.score);
        }

        private void SetLocalLowApxScrID(int podzialID,int half)
        {
            
            if (half == 0)
            {
                LOCALlowestApxScoreID[half] = 0;
                for (int k = 0; k < podzialID; k++)
                    if (AproxesCopy[k].score > AproxesCopy[LOCALlowestApxScoreID[half]].score) LOCALlowestApxScoreID[half] = k;
            }
            else
            {
                LOCALlowestApxScoreID2 =  podzialID;
                LOCALlowestApxScoreID[half] = podzialID;
                for (int k = podzialID; k < Aproxes.Count; k++) {
                    if (Aproxes[k].score > Aproxes[LOCALlowestApxScoreID[half]].score) LOCALlowestApxScoreID[half] = k;
                    if (Aproxes[k].score > Aproxes[LOCALlowestApxScoreID2].score) LOCALlowestApxScoreID2 = k;
                }
            }
            
        }
        private void SetLocalLowApxScrID2(int podzialID, int half)
        {

            if (half == 0)
            {
                LOCALlowestApxScoreID[half] = 0;
                for (int k = 0; k < podzialID; k++)
                    if (AproxesCopy[k].score > AproxesCopy[LOCALlowestApxScoreID[half]].score) LOCALlowestApxScoreID[half] = k;
            }
            else
            {
                LOCALlowestApxScoreID2 = podzialID;
                LOCALlowestApxScoreID[half] = podzialID;
                for (int k = podzialID; k < Aproxes.Count; k++)
                {
                    if (Aproxes[k].score > Aproxes[LOCALlowestApxScoreID[half]].score) LOCALlowestApxScoreID[half] = k;
                    if (Aproxes[k].score > Aproxes[LOCALlowestApxScoreID2].score) LOCALlowestApxScoreID2 = k;
                }
            }

        }

        //private void SetLowApxScrID()
        //{
        //    lowestApxScoreID = 0;
        //    for (int k = 0; k < Aproxes.Count; k++)
        //        if (Aproxes[k].score > Aproxes[lowestApxScoreID].score) lowestApxScoreID = k;
        //}
        private void pictureBoxChoosen_Click(object sender, EventArgs e)
        {
            pictureBoxSelected.Image = ((PictureBox)sender).Image;
            tabControl1.SelectedIndex = 3;
            tabControl1.SelectedTab.Text = Aproxes[flowLayoutPanel1.Controls.GetChildIndex(((PictureBox)sender))].Generation.ToString();
        }
        private void pictureBoxOrginal_Click(object sender, EventArgs e)
        {
            // pictureBoxSelected.Image = ((PictureBox)sender).Image;
            tabControl1.SelectedIndex = 3;
        }
        private void pictureBoxSelected_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void Scores_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Aprox apx in Aproxes)
            {
                if (((int)Scores.SelectedItem) == apx.score)
                {
                    pictureBoxSelected.Image = apx.madeImage;
                    break;
                }
            }
            tabControl1.SelectedIndex = 3;
            tabControl1.SelectedTab.Text = Aproxes[Scores.Items.IndexOf(((int)sender))].Generation.ToString();
        }

        //private delegate void SetControlPropertyThreadSafeDelegate(
        //    Control control,
        //    string propertyName,
        //    object propertyValue);

        //public static void SetControlPropertyThreadSafe(
        //    Control control,
        //    string propertyName,
        //    object propertyValue)
        //{
        //    if (control.InvokeRequired)
        //    {
        //        control.Invoke(new SetControlPropertyThreadSafeDelegate
        //        (SetControlPropertyThreadSafe),
        //        new object[] { control, propertyName, propertyValue });
        //    }
        //    else
        //    {
        //        control.GetType().InvokeMember(
        //            propertyName,
        //            BindingFlags.SetProperty,
        //            null,
        //            control,
        //            new object[] { propertyValue });
        //    }
        //}
    }
}
