using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;

namespace MacroTest
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
            cboPort.Items.AddRange(ports);
            cboPort.SelectedIndex = 0;
            cmdConnect.Enabled = true;
            cmdDisconnect.Enabled = false;

            // load settings
            loadDefaults();

        }
        private void loadDefaults()
        {
            for (int i = 1; i <= 15; i++)
            {
                string hexValue = i.ToString("X"); // convert to hex for organization

                // Find text control
                string gbName = "groupBox" + hexValue;
                GroupBox groupName = (GroupBox)this.Controls[gbName];
                string ctrlName = "key" + hexValue + "Text";
                TextBox myText = (TextBox)groupName.Controls[ctrlName];
                //TextBox myText = (TextBox)this.Controls[ctrlName];

                // Find radio button control
                string rbctrlName = "key" + hexValue + "HotKey";
                bool HotKey = Convert.ToBoolean(Properties.Settings.Default[rbctrlName].ToString());


                // Set defaults
                myText.Text = Properties.Settings.Default[ctrlName].ToString();
                if (HotKey)
                {
                    RadioButton myButton = (RadioButton)groupName.Controls[rbctrlName];
                    myButton.Checked = true;
                }
                else
                {
                    rbctrlName = "key" + hexValue + "App";
                    RadioButton myButton = (RadioButton)groupName.Controls[rbctrlName];
                    myButton.Checked = true;
                }
            }
        }
        private void saveDefaults() {
            for (int i = 1; i <= 15; i++)
            {
                string hexValue = i.ToString("X");  // convert to letters to make organization simpler

                // Get command information
                string gbName = "groupBox" + hexValue;
                GroupBox groupName = (GroupBox)this.Controls[gbName];
                string ctrlName = "key" + hexValue + "Text";
                TextBox myText = (TextBox)groupName.Controls[ctrlName];
                //TextBox myText = (TextBox)this.Controls[ctrlName];

                // Get radio button details
                string rbctrlName = "key" + hexValue + "HotKey";
                RadioButton myButton = (RadioButton)groupName.Controls[rbctrlName];

                // save settings
                Properties.Settings.Default[ctrlName] = myText.Text;
                if (myButton.Checked)
                {
                    Properties.Settings.Default[rbctrlName] = true;
                }
                else
                {
                    Properties.Settings.Default[rbctrlName] = false;
                }
                Properties.Settings.Default.Save();
            }
        }
        private void cmdConnect_Click(object sender, EventArgs e)
        {
            cmdConnect.Enabled = false;
            cmdDisconnect.Enabled = true;
            try
            {
                serialPort1.PortName = cboPort.Text;
                serialPort1.Open();
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();

                serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdDisconnect_Click(object sender, EventArgs e)
        {
            // disconnect from serial port
            cmdConnect.Enabled = true;
            cmdDisconnect.Enabled = false;
            try
            {
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();
                serialPort1.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
                serialPort1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // save settings persistently
            saveDefaults();
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // get serial data and trim off CRLF
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting().ToUpper();
            indata = indata.Trim('\r');
            indata = indata.Trim('\n');
            indata = indata.Trim('\r');

            try
            {
                string command = "";
                bool appRadio = false;

                // get command text
                string ctrlName = "key" + indata + "Text";
                string gbName = "groupBox" + indata;
                GroupBox groupName = (GroupBox)this.Controls[gbName];
                TextBox myText = (TextBox)groupName.Controls[ctrlName];
                //TextBox myText = (TextBox)this.Controls[ctrlName];
                command = myText.Text;

                // Get radio button
                ctrlName = "key" + indata + "HotKey";
                //bool HotKey = Convert.ToBoolean(Properties.Settings.Default[ctrlName].ToString());
                RadioButton myButton = (RadioButton)groupName.Controls[ctrlName];
                appRadio = myButton.Checked;

                // if command is empty, prompt with message
                // if appRadio is true, run command. Otherwise run hotkey
                if (command == "")
                {
                    MessageBox.Show(indata, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (appRadio)
                {
                    SendKeys.SendWait(command);
                }
                else
                {
                    Process.Start(command);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
