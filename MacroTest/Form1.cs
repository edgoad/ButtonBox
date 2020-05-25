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
                string hexValue = i.ToString("X");
                string ctrlName = "key" + hexValue + "Text";
                TextBox myText = (TextBox)this.Controls[ctrlName];
                //myText.Text = Properties.Settings.Default.key1Text;
                myText.Text = Properties.Settings.Default[ctrlName].ToString();
                
                ctrlName = "key" + hexValue + "HotKey";
                string gbName = "groupBox" + hexValue;
                bool HotKey = Convert.ToBoolean(Properties.Settings.Default[ctrlName].ToString());
                GroupBox groupName = (GroupBox)this.Controls[gbName];
                if (HotKey)
                {
                    RadioButton myButton = (RadioButton)groupName.Controls[ctrlName];
                    myButton.Checked = true;
                }
                else
                {
                    ctrlName = "key" + hexValue + "App";
                    RadioButton myButton = (RadioButton)groupName.Controls[ctrlName];
                    myButton.Checked = true;
                }
            }
        }
        private void saveDefaults() {
            for (int i = 1; i <= 15; i++)
            {
                string hexValue = i.ToString("X");
                string ctrlName = "key" + hexValue + "Text";
                TextBox myText = (TextBox)this.Controls[ctrlName];
                //myText.Text = Properties.Settings.Default.key1Text;
                //myText.Text = Properties.Settings.Default[ctrlName].ToString();
                Properties.Settings.Default[ctrlName] = myText.Text;

                ctrlName = "key" + hexValue + "HotKey";
                string gbName = "groupBox" + hexValue;
                //bool HotKey = Convert.ToBoolean(Properties.Settings.Default[ctrlName].ToString());
                GroupBox groupName = (GroupBox)this.Controls[gbName];
                RadioButton myButton = (RadioButton)groupName.Controls[ctrlName];
                if (myButton.Checked)
                {
                    Properties.Settings.Default[ctrlName] = true;
                }
                else
                {
                    Properties.Settings.Default[ctrlName] = false;
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
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting().ToUpper();
            indata = indata.Trim('\r');
            indata = indata.Trim('\n');
            indata = indata.Trim('\r');

            //SetText(indata);
            //txtSerial.AppendText(indata);
            try
            {
                string command = "";
                bool appRadio = false;
                //switch (indata)
                //{
                //    case "1":
                //        command = key1Text.Text.Trim();
                //        if (key1App.Checked)
                //        {
                //            appRadio = true;
                //        }
                //        else
                //        {
                //            appRadio = false;
                //        }
                //        break;
                //    case "2":
                //        command = key2Text.Text.Trim();
                //        if (command == "")
                //        {
                //            MessageBox.Show(indata, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        }
                //        else if (key2App.Checked)
                //        {
                //            Process.Start(command);
                //        }
                //        else
                //        {
                //            SendKeys.SendWait(command);
                //        }
                //        break;

                //    //case "3":
                //    //    break;
                //    //case "4":
                //    //    break;
                //    //case "5":
                //    //    break;
                //    //case "6":
                //    //    break;
                //    //case "7":
                //    //    break;
                //    //case "8":
                //    //    break;
                //    //case "9":
                //    //    break;
                //    //case "a":
                //    //    break;
                //    //case "b":
                //    //    break;
                //    //case "c":
                //    //    break;
                //    //case "d":
                //    //    break;
                //    //case "e":
                //    //    break;
                //    //case "f":
                //    //    break;
                //    default:
                //        //MessageBox.Show(indata, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        break;
                //}

                // get command text
                string ctrlName = "key" + indata + "Text";
                TextBox myText = (TextBox)this.Controls[ctrlName];
                command = myText.Text;

                // Get radio button
                ctrlName = "key" + indata + "HotKey";
                string gbName = "groupBox" + indata;
                //bool HotKey = Convert.ToBoolean(Properties.Settings.Default[ctrlName].ToString());
                GroupBox groupName = (GroupBox)this.Controls[gbName];
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
