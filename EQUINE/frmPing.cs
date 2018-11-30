﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace EQUINE
{
    public partial class frmPing : Form
    {
        Ping pingSender = new Ping();
        string errMsg = "No errors.";
        AutoResetEvent waiter;

        public frmPing()
        {
            InitializeComponent();
            
        }

        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                SetText("Ping failed! Click Show Info for details...");
                System.Media.SystemSounds.Hand.Play();
                errMsg = e.Error.ToString();
            }

            PingReply reply = e.Reply;
            if (reply != null)
            {
                if (reply.Status == IPStatus.Success)
                {
                    SetText("Success! Remote host is visible from your PC! Time: " + reply.RoundtripTime + " ms");
                    System.Media.SystemSounds.Exclamation.Play();
                    errMsg = "Ping info:\n" + "TripTime: " + reply.RoundtripTime + "\nBuffer size: " + reply.Buffer.Length + "\nLifeTime: " + reply.Options.Ttl;
           
                }
                else
                {
                    SetText("Ping failed! No reply.");
                    System.Media.SystemSounds.Hand.Play();
                }
            }
            ((AutoResetEvent)e.UserState).Set();
            DisablePingButton(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("IP/Remotehost field must not be empty.", "");
                return;
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void frmPing_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(errMsg, "Ping Log", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        delegate void SetTextCallback(string text);
        delegate void DisableButton(bool dis);

        private void SetText(string text)
        {
            if (label3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text });
            }
            else
            {
                label3.Text = text;
            }
        }

        private void DisablePingButton(bool ornot)
        {
            if(button1.InvokeRequired)
            {
                DisableButton d = new DisableButton(DisablePingButton);
                Invoke(d, new object[] { ornot });
            }
            else
            {
                button1.Enabled = ornot;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DisablePingButton(false);
            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
            waiter = new AutoResetEvent(false);

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            int timeout = 12000;
            PingOptions options = new PingOptions(64, true);

            pingSender.SendAsync(textBox1.Text, timeout, buffer, options, waiter);
            SetText("Pinging...");
            waiter.WaitOne();
        }
    }
}
