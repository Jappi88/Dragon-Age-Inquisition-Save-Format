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
                    byte[] data = null;
                    using (var fs = new FileStream(ofd.FileName, FileMode.Open))
                    {
                        using (var x = File.Create(ofd.FileName + ".Rebuilded"))
                        {
                            SaveData.SaveFormat s = new SaveFormat(fs);
                            data = s.Rebuild();
                            x.Write(data, 0, data.Length);
                        }
                    }
                    if (!data.MemCompare(File.ReadAllBytes(ofd.FileName), 0))
                        MessageBox.Show(@"Invalid Comparison!");
                    else
                        MessageBox.Show("Rebuilded File Matches Original!\n Yeeaahhh :P!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}