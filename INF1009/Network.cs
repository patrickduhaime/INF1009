using System.Threading;
using System.Collections;
using System.IO;
using System;
using System.Timers;

namespace INF1009
{
    class Network
    {
        private Queue transport2Network;
        private Queue network2Transport;
        private Queue network2PacketProcessing;
        private Queue packetProcessing2Network;
        private const string L_lec = "l_lec.txt";
        private const string L_ecr = "l_ecr.txt";
        private FileStream fileFromTransport;
        private FileStream file2Transport;
        private StreamWriter writeFromTransport;
        private StreamWriter write2Transport;
        private int respAddr, sentCount;
        byte[] sourceAddr;
        byte[] destAddr;
        byte[] outputNo;
        byte pr;
        bool expired, rejected, accepted, disconnected, connected;
        System.Timers.Timer timer;
        string receivedData;

        public Network(ref Queue transport2Network, ref Queue network2Transport, ref Queue packetProcessing2Network, ref Queue network2PacketProcessing)
        {
            this.transport2Network = transport2Network;
            this.network2Transport = network2Transport;
            this.network2PacketProcessing = network2PacketProcessing;
            this.packetProcessing2Network = packetProcessing2Network;

            fileFromTransport = new FileStream(L_ecr, FileMode.OpenOrCreate, FileAccess.Write);
            writeFromTransport = new StreamWriter(fileFromTransport);
            file2Transport = new FileStream(L_lec, FileMode.OpenOrCreate, FileAccess.Write);
            write2Transport = new StreamWriter(file2Transport);
            Start();
        }

        public void resetFiles()
        {
            fileFromTransport.Position = 0;
            file2Transport.Position = 0;
        }

        public void Start()
        {
            Random rnd = new Random();

            writeFromTransport.Flush();
            write2Transport.Flush();

            sourceAddr = new byte[1];
            destAddr = new byte[1];
            outputNo = new byte[1];
            newConnection(outputNo);

            respAddr = rnd.Next();
            pr = 0;
            receivedData = "";
            disconnected = false;
            connected = false;

            timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += onTimeEvent;
            timer.AutoReset = true;
        }

        public void transportWrite()
        {
            while (true)
            {
                try
                {
                    if (!disconnected)
                    {
                        if (packetProcessing2Network.Count > 0)
                        {
                            if (packetProcessing2Network.Peek().GetType() == typeof(byte[]))
                            {
                                string msg = "";
                                byte[] received = (byte[])packetProcessing2Network.Dequeue();
                                PACKET receivedPacket = Packet.decapBytes(received);
                                Npdu _4Transport = Packet.decapPacket(receivedPacket);
                                if (_4Transport.type == "WrongPacketFormat")
                                {
                                    msg = "Wrong Packet Format";
                                }
                                else if (_4Transport.type == "NACK")
                                {
                                    msg = "NACK  negative Acknowledgment  :" + receivedPacket.packetType.ToString();
                                    rejected = true;
                                }
                                else if (_4Transport.type == "ACK")
                                {
                                    msg = "ACK  positive Acknowledgment :" + receivedPacket.packetType.ToString();
                                    accepted = true;
                                }
                                else if(_4Transport.type == "N_DISCONNECT.ind")
                                {
                                    msg = "N_DISCONNECT " + _4Transport.target;
                                    network2Transport.Enqueue(_4Transport);
                                    accepted = true;
                                    disconnected = true;
                                }
                                else if(_4Transport.type == "N_CONNECT.ind")
                                {
                                    msg = "N_CONNECT  dest Address :" + _4Transport.destAddr + " source Address: " + _4Transport.sourceAddr;
                                    network2Transport.Enqueue(_4Transport);
                                    accepted = true;
                                }
                                else if(_4Transport.type == "N_DATA.ind")
                                {
                                    msg = "N_DATA  transferring network data";
                                    receivedData += _4Transport.data;
                                    if (!_4Transport.flag)
                                    {
                                        _4Transport.data = receivedData;
                                        network2Transport.Enqueue(_4Transport);
                                    }
                                }

                                write2Transport.WriteLine(msg);
                                Form1._UI.write2L_ecr(msg);
                            }
                        }
                    }
                }
                catch (ThreadAbortException)
                {

                }
            }
        }

        public void transportRead()
        {
            while (true)
            {
                try
                {
                    if (!disconnected)
                    {
                        if (transport2Network.Count > 0)
                        {
                            if (transport2Network.Peek().GetType() == typeof(Npdu))
                            {
                                Npdu transportNpdu = (Npdu)transport2Network.Dequeue();
                                Npdu npdu2Transport;
                                PACKET packet4Processing;
                                string msg;

                                if (transportNpdu.type != "N_CONNECT.req" && !connected) { }
                                else
                                {
                                    if (transportNpdu.type == "N_CONNECT.req")

                                    {
                                        msg = "N_CONNECT " + transportNpdu.destAddr + " " + transportNpdu.sourceAddr + " route: " + transportNpdu.routeAddr;
                                        writeFromTransport.WriteLine(msg);
                                        Form1._UI.write2L_lec(msg);

                                        sourceAddr[0] = (byte)int.Parse(transportNpdu.sourceAddr);
                                        destAddr[0] = (byte)int.Parse(transportNpdu.destAddr);
                                        if (sourceAddr[0] % 27 == 0 || int.Parse(transportNpdu.sourceAddr) > 249 || int.Parse(transportNpdu.destAddr) > 249)
                                        {
                                            disconnected = true;
                                            connected = false;

                                            npdu2Transport = new Npdu();
                                            npdu2Transport.type = "N_DISCONNECT.ind";
                                            npdu2Transport.routeAddr = respAddr.ToString();
                                            npdu2Transport.target = "00000010";
                                            npdu2Transport.connection = "255";
                                            network2Transport.Enqueue(npdu2Transport);
                                        }
                                        else
                                        {
                                            connected = true;

                                            npdu2Transport = new Npdu();
                                            npdu2Transport.type = "N_CONNECT.conf";
                                            npdu2Transport.routeAddr = respAddr.ToString();
                                            npdu2Transport.sourceAddr = sourceAddr[0].ToString();
                                            npdu2Transport.destAddr = destAddr[0].ToString();
                                            npdu2Transport.connection = outputNo[0].ToString();
                                            network2Transport.Enqueue(npdu2Transport);

                                            packet4Processing = Packet.encapsulateRequest(outputNo[0], sourceAddr[0], destAddr[0]);
                                            string packetType = "request";
                                            byte[] sending = Packet.encapsulateBytes(packet4Processing, packetType);

                                            sentCount = 0;
                                            rejected = false;
                                            expired = true;
                                            accepted = false;
                                            timer.Start();
                                            while (!accepted && sentCount < 2)
                                            {
                                                if (expired || rejected)
                                                {
                                                    network2PacketProcessing.Enqueue(sending);
                                                    expired = false;
                                                    rejected = false;
                                                    sentCount++;
                                                }
                                            }
                                            timer.Stop();
                                        }
                                    }
                                    else if (transportNpdu.type == "N_DATA.req")
                                    {
                                        msg = "N_DATA " + transportNpdu.data;
                                        writeFromTransport.WriteLine(msg);
                                        Form1._UI.write2L_lec(msg);

                                        PACKET[] packets4Processing = Packet.encapsulateFullData(transportNpdu.data, outputNo[0], pr);
                                        foreach (PACKET packet in packets4Processing)
                                        {
                                            byte[] sending = Packet.encapsulateDataBytes(packet);

                                            sentCount = 0;
                                            rejected = false;
                                            expired = true;
                                            accepted = false;
                                            timer.Start();
                                            while (!accepted && sentCount < 2)
                                            {
                                                if (expired || rejected)
                                                {
                                                    network2PacketProcessing.Enqueue(sending);
                                                    expired = false;
                                                    rejected = false;
                                                    sentCount++;
                                                }
                                            }
                                            timer.Stop();
                                        }
                                    }
                                    else if (transportNpdu.type == "N_DISCONNECT.req")
                                    {
                                        msg = "N_DISCONNECT " + transportNpdu.routeAddr;
                                        writeFromTransport.WriteLine(msg);
                                        Form1._UI.write2L_lec(msg);

                                        npdu2Transport = new Npdu();
                                        npdu2Transport.type = "N_DISCONNECT.ind";
                                        npdu2Transport.routeAddr = respAddr.ToString();

                                        packet4Processing = Packet.encapsulateRelease(outputNo[0], sourceAddr[0], destAddr[0], true);
                                        string packetType = "release";
                                        byte[] sending = Packet.encapsulateBytes(packet4Processing, packetType);

                                        sentCount = 0;
                                        rejected = false;
                                        expired = true;
                                        accepted = false;
                                        timer.Start();
                                        while (!accepted && sentCount < 2)
                                        {
                                            if (expired || rejected)
                                            {
                                                network2PacketProcessing.Enqueue(sending);
                                                expired = false;
                                                rejected = false;
                                                sentCount++;
                                            }
                                        }
                                        timer.Stop();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (ThreadAbortException) { }
            }
        }

        private void onTimeEvent(object sender, ElapsedEventArgs e)
        {
            expired = true;
        }

        private void newConnection(byte[] connectionNumber)
        {
            Random rnd = new Random();

            rnd.NextBytes(connectionNumber);
            connectionNumber[0] %= 8;
        }
    }
}
