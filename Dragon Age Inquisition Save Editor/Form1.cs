#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Dragon_Age_Inquisition_Save_Editor.DAIO;
using Dragon_Age_Inquisition_Save_Editor.SaveData;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sf = File.ReadAllBytes(ofd.FileName);
                    var s = new SaveFormat(ofd.FileName);
                    var x = s.Rebuild();
                    File.WriteAllBytes(ofd.FileName + ".New", x);
                    propertyGrid1.SelectedObject = s.DataStructure;
                    int offset = 0;
                    Dictionary<int, byte> fault = new Dictionary<int, byte>();
                    Dictionary<int, byte> valid = new Dictionary<int, byte>();
                    var same = sf.MemCompare(x, 0, ref fault,ref valid);
                    Console.Write(same + @"  ==> 0x" + offset.ToString("X2"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var fs = new FileStream(ofd.FileName, FileMode.Open))
                    {
                        var io = new DAIIO(fs,true, 0, 0);
                        using (var x = File.Create(ofd.FileName + ".NewFile"))
                        {
                            int count = 0;
                            var buf = new byte[0x1000];
                            while ((count = io.Read(buf, 0, 0x1000)) > 0)
                                x.Write(buf, 0, count);
                        }
                    }
                    MessageBox.Show(@"Done!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}