﻿using System;
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

//TODO: find out why connection intermittenly fails and must be unplugged/replugged
// TODO: Have autoconnect
namespace MacroTest
{
    public partial class MacroTest : Form
    {
        bool showForm = true;
        bool isConnected;
        public MacroTest()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            isConnected = false;
            try
            {
                string[] ports = SerialPort.GetPortNames();
                cboPort.Items.AddRange(ports);
                cboPort.SelectedIndex = 0;
                cmdConnect.Enabled = true;
                cmdDisconnect.Enabled = false;
            }
            catch(Exception ex)
            { }

            // load settings
            loadDefaults();

            // set notify icon
            UpdateConnectStatus();
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

            ComboBox port = (ComboBox)this.Controls["cboPort"];
            port.SelectedItem = Properties.Settings.Default["COMPort"];
            CheckBox _autoConnect = (CheckBox)this.Controls["chkAutoConnect"];
            bool autoConnect = Convert.ToBoolean(Properties.Settings.Default["AutoConnect"]);
            _autoConnect.Checked = autoConnect;
            if (autoConnect)
            {
                ConnectPort();
            }
        }
        private void saveDefaults()
        {
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
            }
            ComboBox port = (ComboBox)this.Controls["cboPort"];
            Properties.Settings.Default["COMPort"] = port.Text;
            CheckBox autoConnect = (CheckBox)this.Controls["chkAutoConnect"];
            Properties.Settings.Default["AutoConnect"] = autoConnect.Checked;

            Properties.Settings.Default.Save();
        }
        private void cmdConnect_Click(object sender, EventArgs e)
        {
            ConnectPort();    
        }
        private void ConnectPort()
        {
            appendLog("Attempting to connect to " + cboPort.Text);
            try
            {
                serialPort1.PortName = cboPort.Text;
                serialPort1.Open();
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();

                serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                appendLog("Connected to " + cboPort.Text);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //CheckBox _autoConnect = (CheckBox)this.Controls["chkAutoConnect"];
                appendLog("Could not connect to port");
            }

            // set notify icon
            UpdateConnectStatus();
        }
        private void cmdDisconnect_Click(object sender, EventArgs e)
        {
            // disconnect from serial port
            //cmdConnect.Enabled = true;
            //cmdDisconnect.Enabled = false;
            appendLog("Disconnecting from " + cboPort.Text);
            try
            {
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();
                serialPort1.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
                serialPort1.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appendLog("Failed to disconnect from " + cboPort.Text);
            }

            // save settings persistently
            saveDefaults();

            // set notify icon
            UpdateConnectStatus();

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
                    //MessageBox.Show(indata, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    appendLog("Unmapped button pushed: " + indata);
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
                //MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appendLog(ex.Message);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ToggleFormVisibility();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            ToggleFormVisibility();

            //if (this.WindowState == FormWindowState.Normal)
            //{
            //    Hide();
            //    this.WindowState = FormWindowState.Minimized;
            //}
            //else
            //{
            //    Show();
            //    this.WindowState = FormWindowState.Normal;
            //}
        }
        private void UpdateConnectStatus()
        {
            if (serialPort1.IsOpen)
            {
                notifyIcon1.Text = "MacroTest (connected)";
                cmdConnect.Enabled = false;
                cmdDisconnect.Enabled = true;
                isConnected = true;
            }
            else
            {
                notifyIcon1.Text = "MacroTest (disconnected)";
                cmdConnect.Enabled = true;
                cmdDisconnect.Enabled = false;
                isConnected = false;
            }
        }
        private void ToggleFormVisibility() {
            if (showForm)
            {
                Hide();
                showForm = false;
            }
            else
            {
                Show();
                this.WindowState = FormWindowState.Normal;
                showForm = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                if (lblConnected.Text != "Connected")
                {
                    UpdateConnectStatus();
                    lblConnected.Text = "Connected";
                    lblConnected.BackColor = Color.Green;
                }
            }
            else {
                if (chkAutoConnect.Checked)
                {
                    ConnectPort();
                }
                if (serialPort1.IsOpen ==false & lblConnected.Text != "Disconnected")
                {
                    UpdateConnectStatus();
                    lblConnected.Text = "Disconnected";
                    lblConnected.BackColor = Color.Red;
                }
            }
        }
        //private void appendLog(string message)
        //{
        //    string date = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
        //    this.txtLog.AppendText(date + " - " + message + "\r\n");
        //}
        delegate void SetTextCallback(string text);

        private void appendLog(string message)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(appendLog);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                //this.textBox1.Text = text;
                string date = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
                txtLog.AppendText(date + " - " + message + "\r\n");
            }
        }
    }
}
