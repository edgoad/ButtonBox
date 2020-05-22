using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cmbPort.Items.AddRange(ports);
            cmbPort.SelectedIndex = 0;
            cmdClose.Enabled = false;

        }

        private void cmdOpen_Click(object sender, EventArgs e)
        {
            cmdOpen.Enabled = false;
            cmdClose.Enabled = true;
            try
            {
                serialPort1.PortName = cmbPort.Text;
                serialPort1.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            cmdOpen.Enabled = true;
            cmdClose.Enabled = false;
            try
            {
                serialPort1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdReceive_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    txtSerial.Text = serialPort1.ReadExisting();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
