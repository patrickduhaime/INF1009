﻿using System;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace INF1009
{
    public partial class Form1 : Form
    {
        public static Form1 _UI;
        private int nbclk = 0;
        private int nbTest = 1;
        private static Queue N2TQ = new Queue();
        private static Queue N2TQS = Queue.Synchronized(N2TQ);
        private static Queue T2NQ = new Queue();
        private static Queue T2NQS = Queue.Synchronized(T2NQ);
        private static Queue N2PPQ = new Queue();
        private static Queue N2PPQS = Queue.Synchronized(N2PPQ);
        private static Queue PP2NQ = new Queue();
        private static Queue PP2NQS = Queue.Synchronized(PP2NQ);
        private Processing processing = new Processing(ref PP2NQS, ref N2PPQS);
        private Network network = new Network(ref T2NQS, ref N2TQS, ref PP2NQS, ref N2PPQS);
        private Transport transport = new Transport(ref T2NQS, ref N2TQS);
        private Thread networkWriteThread, transportWriteThread, transportReadThread, networkReadThread, processingThread;
        delegate void UIDisplayText(string text);

        public Form1()
        {
            InitializeComponent();
            _UI = this;
            networkWriteThread = new Thread(new ThreadStart(transport.networkWrite));
            networkReadThread = new Thread(new ThreadStart(transport.networkRead));
            transportWriteThread = new Thread(new ThreadStart(network.transportWrite));
            transportReadThread = new Thread(new ThreadStart(network.transportRead));
            processingThread = new Thread(new ThreadStart(processing.startProcessing));
        }


        private void openThreads()
        {
            networkWriteThread = new Thread(new ThreadStart(transport.networkWrite));
            networkWriteThread.Name = "networkWriteThread";
            networkWriteThread.Start(); ;

            networkReadThread = new Thread(new ThreadStart(transport.networkRead));
            networkReadThread.Name = "networkReadThread";
            networkReadThread.Start(); ;

            transportWriteThread = new Thread(new ThreadStart(network.transportWrite));
            transportWriteThread.Name = "transportWriteThread";
            transportWriteThread.Start();

            transportReadThread = new Thread(new ThreadStart(network.transportRead));
            transportReadThread.Name = "transportReadThread";
            transportReadThread.Start();

            processingThread = new Thread(new ThreadStart(processing.startProcessing));
            processingThread.Name = "processingThread";
            processingThread.Start();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            nbclk++;
            if (nbclk > 6)
                buttonStart.Enabled = false;
            try
            {
                reset();
                openThreads();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

        public void reset()
        {
            if (processingThread.ThreadState == ThreadState.Running ||
            networkWriteThread.ThreadState == ThreadState.Running ||
            transportWriteThread.ThreadState == ThreadState.Running ||
            networkReadThread.ThreadState == ThreadState.Running ||
            transportReadThread.ThreadState == ThreadState.Running)
            {
                closeThreads();
            }
            T2NQS.Clear();
            N2TQS.Clear();
            PP2NQS.Clear();
            N2PPQS.Clear();
            network.Start();
            transport.Start();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = true;
            nbclk = 0;
            rtbL_ecr.Clear();
            rtbL_lec.Clear();
            rtbS_ecr.Clear();
            rtbS_lec.Clear();
            reset();
            network.resetFiles();
            transport.resetFiles();
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            closeThreads();
            this.Close();
        }

        private void form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeThreads();
        }

        public void closeThreads()
        {
            processingThread.Abort();
            networkReadThread.Abort();
            networkWriteThread.Abort();
            transportReadThread.Abort();
            transportWriteThread.Abort();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            richTextBoxGen.Clear();
            string dest = transport.setDestAddress();
            int intDest = Int32.Parse(dest);
            string source = transport.setSourceAddress(intDest);

            richTextBoxGen.AppendText("N_CONNECT " + dest + " " + source + "\n");
            richTextBoxGen.AppendText("N_DATA test no.: " + nbTest + "\n");
            richTextBoxGen.AppendText("N_DISCONNECT " + dest + " " + source + "\n");
        }

        private void buttonSend2File_Click(object sender, EventArgs e)
        {
            richTextBoxGen.Clear();
            nbTest++;

            richTextBoxGen.SaveFile("t_lec.txt");
            transport.networkTest();
        }

        public void write2L_lec(string text)
        {
            string txt = text + Environment.NewLine;

            if (this.rtbL_lec.InvokeRequired)
            {
                UIDisplayText displayText = new UIDisplayText(write2L_lec);
                this.Invoke(displayText, new object[] { text });
            }
            else
            {
                rtbL_lec.AppendText(txt);
            }
        }

        public void write2S_lec(string text)
        {
            string txt = text + Environment.NewLine;
            if (this.rtbS_lec.InvokeRequired)
            {
                UIDisplayText displayText = new UIDisplayText(write2S_lec);
                this.Invoke(displayText, new object[] { text });
            }
            else
            {
                rtbS_lec.AppendText(txt);
            }
        }


        public void write2S_ecr(string text)
        {
            string txt = text + Environment.NewLine;
            if (this.rtbL_lec.InvokeRequired)
            {
                UIDisplayText displayText = new UIDisplayText(write2S_ecr);
                this.Invoke(displayText, new object[] { text });
            }
            else
            {
                rtbS_ecr.AppendText(txt);
            }
        }

        public void write2L_ecr(string text)
        {
            string txt = text + Environment.NewLine;
            if (this.rtbL_lec.InvokeRequired)
            {
                UIDisplayText displayText = new UIDisplayText(write2L_ecr);
                this.Invoke(displayText, new object[] { text });
            }
            else
            {
                rtbL_ecr.AppendText(txt);
            }
        }
    }
}
