using System.IO;
using System.Threading;
using System.Collections;

namespace INF1009
{
    class Transport
    {

        private const string S_lec = "s_lec.txt";
        private const string S_ecr = "s_ecr.txt";
        private StreamReader reader;
        private StreamWriter writer;
        private FileStream inputFile, outputFile;
        private Queue transport2Network, network2Transport;
        string msg;
        bool disconnect,end, pause;
        ArrayList connected;

        public Transport(ref Queue transport2Network, ref Queue network2Transport)
        {

            this.transport2Network = transport2Network;
            this.network2Transport = network2Transport;

            inputFile = new FileStream(S_lec, FileMode.OpenOrCreate, FileAccess.Read);
            reader = new StreamReader(inputFile);
            outputFile = new FileStream(S_ecr, FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(outputFile);

            connected = new ArrayList();
            end = false;
            Init();
        }

        public void resetFiles()
        {
            connected.Clear();
            reader.DiscardBufferedData();
            end = false;
            inputFile.Position = 0;
            outputFile.Position = 0;
        }

        public void Init()
        {
            writer.Flush();
            msg = "";
            disconnect = false;
            pause = false;
        }
        public void networkWrite()
        {
            string lineRead;
            string[] settings;
            Npdu networkNNpdu;
            bool valid;

            while (!end && !disconnect)
            {
               
                    if ((lineRead = reader.ReadLine()) != null)
                    {
                        networkNNpdu = new Npdu();

                        try
                        {
                            valid = false;

                            settings = lineRead.Split(' ');

                            Form1._UI.write2S_lec(lineRead);

                            switch (settings[0])
                            {
                                case "N_CONNECT":
                                    {
                                        networkNNpdu.type = "N_CONNECT.req";
                                        networkNNpdu.sourceAddr = settings[1];
                                        networkNNpdu.destAddr = settings[2];
                                        valid = true;
                                        break;
                                    }
                                case "N_DATA":
                                    {
                                        networkNNpdu.type = "N_DATA.req";
                                        for (int i = 1; i < settings.Length; i++)
                                            networkNNpdu.data += settings[i] + " ";
                                        valid = true;
                                        break;
                                    }
                                case "N_DISCONNECT":
                                    {
                                        networkNNpdu.type = "N_DISCONNECT.req";
                                        networkNNpdu.routeAddr = settings[1];
                                        valid = true;
                                        disconnect = true;
                                        break;
                                    }
                            }
                            if (valid)
                                transport2Network.Enqueue(networkNNpdu);
                        }
                        catch (ThreadAbortException)
                        {

                        }
                    }
                    else
                    {
                        end = true;
                    }
                
            }
        }


        public void networkRead()
        {
            while (true)
            {
                try
                {
                    if (network2Transport.Count > 0)
                    {
                        if (network2Transport.Peek().GetType() == typeof(Npdu))
                        {
                            Npdu Npdu4Network = (Npdu)network2Transport.Dequeue();

                            switch (Npdu4Network.type)
                            {
                                case "N_CONNECT.ind":
                                    {
                                        if (connected.Contains(Npdu4Network.connection))
                                        { 
                                        msg = "source Address: " + Npdu4Network.sourceAddr + "  dest Address: " + Npdu4Network.destAddr + "  connection number: " + Npdu4Network.connection;
                                        writer.WriteLine(msg);
                                        Form1._UI.write2S_ecr(msg);
                                        }
                                        break;
                                    }
                                case "N_CONNECT.conf":
                                    {
                                        msg = "Connection established " + Npdu4Network.routeAddr + "  connection number: " + Npdu4Network.connection;
                                        writer.WriteLine(msg);
                                        Form1._UI.write2S_ecr(msg);
                                        connected.Add(Npdu4Network.connection);
                                        break;
                                    }
                                case "N_DATA.ind":
                                    {
                                        if (connected.Contains(Npdu4Network.connection))
                                        {
                                            msg = Npdu4Network.data;
                                            writer.WriteLine(msg);
                                            Form1._UI.write2S_ecr(msg);
                                        }
                                        break;
                                    }

                                case "N_DISCONNECT.ind":
                                    {
                                        if (connected.Contains(Npdu4Network.connection))
                                        {
                                            connected.Remove(Npdu4Network.connection);
                                            msg = "Disconnect: " + Npdu4Network.routeAddr + " " + Npdu4Network.target+" connection number: " + Npdu4Network.connection;
                                            writer.WriteLine(msg);
                                            Form1._UI.write2S_ecr(msg);
                                            Form1._UI.closeThreads();
                                        }else if(Npdu4Network.connection.Equals("255")){
                                            Form1._UI.write2S_ecr("Connection declined by Network! " + Npdu4Network.routeAddr);
                                            Form1._UI.closeThreads();
                                        }
                                        break;
                                    }
                            }
                        }

                    }
                }
                catch (ThreadAbortException)
                {

                }

            }
        }
    }
    public struct Npdu
    {
        public string type;
        public string sourceAddr;
        public string destAddr;
        public string routeAddr;
        public string data;
        public string target;
        public string connection;
        public int ps, pr;
        public bool flag;
    }
}
