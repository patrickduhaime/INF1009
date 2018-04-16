﻿using System.IO;
using System.Threading;
using System.Collections;
using System;

namespace INF1009
{
    /**
    * Structure public Npdu qui represente les pacquets
    */
    public struct Npdu
    {
        public string type;
		public string destAddr;
        public string sourceAddr;
        public string routeAddr;
        public string data;
        public string target;
        public string connection;
        public int ps, pr;
        public bool flag;
    }

    /**
    * Classe Transport qui représente la couche transport des systèmes A et B
    */
    class Transport
    {
        private const string S_lec = "s_lec.txt";
        private const string S_ecr = "s_ecr.txt";
        private StreamReader reader;
        private StreamWriter writer;
        private FileStream inputFile;
        private FileStream outputFile;
        private Queue transport2Network;
        private Queue network2Transport;
        string msg;
        bool disconnect;
        ArrayList connected;

        /**
        * Constructeur prenant en parametre les files (FIFO) transport2Network et network2Transport
        */
        public Transport(ref Queue transport2Network, ref Queue network2Transport)
        {

            this.transport2Network = transport2Network;
            this.network2Transport = network2Transport;

            inputFile = new FileStream(S_lec, FileMode.OpenOrCreate, FileAccess.Read);
            reader = new StreamReader(inputFile);
            outputFile = new FileStream(S_ecr, FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(outputFile);

            connected = new ArrayList();
            Start();
        }

        /**
        * Methode Start appelé au demarrage
        */
        public void Start()
        {
            writer.Flush();
            disconnect = false;
            msg = "";
        }

        /**
        * Methode resetFiles qui remets différents paramètres a leurs 
        * valeurs initiales pour la lecture et l'écriture des fichiers.
        */
        public void resetFiles()
        {
            inputFile.Position = 0;
            outputFile.Position = 0;
            connected.Clear();
            reader.DiscardBufferedData();
        }
        /**
        * Methode setRouteAddress definit une addresse de routage selon 
        * l'enonce de travail. Par contre cette adresse n'est pas utilise
        * par le programme, la methode et la variable de routage ne 
        * servent pas, j'ai donc ecris la route dans une variable du Npdu
        */
        private string setRouteAddress(string dest, string source)
        {
            string result = null;

            int r = -1;
            int s = Int32.Parse(source);
            int d = Int32.Parse(dest);

            if (s >= 0 && s <= 99)
            {
                if (d >= 0 && d <= 99)
                    r = d;
                else if (d >= 100 && d <= 199)
                    r = 255;
                else if (d >= 200 && d <= 249)
                    r = 254;
            }
            else if (s >= 100 && s <= 199)
            {
                if (d >= 0 && d <= 99)
                    r = 250;
                else if (d >= 100 && d <= 199)
                    r = d;
                else if (d >= 200 && d <= 249)
                    r = 253;
            }
            else if (s >= 200 && s <= 249)
            {
                if (d >= 0 && d <= 99)
                    r = 251;
                else if (d >= 100 && d <= 199)
                    r = 252;
                else if (d >= 200 && d <= 249)
                    r = d;
            }

            result = result + r;
            return result;
        }

        /**
        * Methode networkWrite (ecrire_vers_reseau) qui lit le fichier s_lec.txt 
        * jusqu'a une ligne qui contiens la String N_DISCONNECT ou jusqu'a ce que 
        * la fin du fichier soit atteint.
        * 
        * Si la ligne lu contiens N_CONNECT, N_DATA ou N_DISCONNECT un pacquet (Npdu)
        * est créer et ajouté a la file transport2Network
        */
        public void networkWrite()
        {
            string lineRead;
            Npdu networkNNpdu;
            string[] settings;
            bool valid, end = false;

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

                        if (settings[0] == "N_CONNECT")
                        {
                            networkNNpdu.type = "N_CONNECT.req";
                            networkNNpdu.destAddr = settings[1];
                            networkNNpdu.sourceAddr = settings[2];
                            networkNNpdu.routeAddr = setRouteAddress(settings[1], settings[2]);
                            valid = true;
                        }
                        else if (settings[0] == "N_DATA")
                        {
                            networkNNpdu.type = "N_DATA.req";
                            for (int i = 1; i < settings.Length; i++)
                                networkNNpdu.data += settings[i] + " ";
                            valid = true;
                        }
                        else if (settings[0] == "N_DISCONNECT")
                        {
                            networkNNpdu.type = "N_DISCONNECT.req";
                            networkNNpdu.routeAddr = settings[1];
                            valid = true;
                            disconnect = true;
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

        /**
        * Methode networkRead (lire_de_reseau) qui verifie si la file network2Transport
        * contiens un Npdu a lire, si tel est le cas le type du Npdu est verifié et un
        * traitement est effectué selon le type trouvé.
        */
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

                            if(Npdu4Network.type == "N_CONNECT.ind")
                            {
                                if (connected.Contains(Npdu4Network.connection))
                                { 
                                msg = "connection: " + Npdu4Network.connection + " dest Address: " + Npdu4Network.destAddr + "  source Address: " + Npdu4Network.sourceAddr;
                                writer.WriteLine(msg);
                                Form1._UI.write2S_ecr(msg);
                                }
                            }
                            else if (Npdu4Network.type == "N_CONNECT.conf")
                            {
                                msg =  "connection: " + Npdu4Network.connection + " Connection established ";
                                writer.WriteLine(msg);
                                Form1._UI.write2S_ecr(msg);
                                connected.Add(Npdu4Network.connection);
                            }
                            else if (Npdu4Network.type == "N_DATA.ind")
                            {
                                if (connected.Contains(Npdu4Network.connection))
                                {
                                    msg = Npdu4Network.data;
                                    writer.WriteLine(msg);
                                    Form1._UI.write2S_ecr(msg);
                                }
                            }
                            else if (Npdu4Network.type == "N_DISCONNECT.ind")
                            {
                                if (connected.Contains(Npdu4Network.connection))
                                {
                                    connected.Remove(Npdu4Network.connection);
                                    msg = "connection: " + Npdu4Network.connection + " " + Npdu4Network.routeAddr + " disconnected " + Npdu4Network.target;
                                    writer.WriteLine(msg);
                                    Form1._UI.write2S_ecr(msg);
                                    Form1._UI.closeThreads();

                                }
                                else if(Npdu4Network.connection.Equals("255"))
                                {
                                    Form1._UI.write2S_ecr("connection: declined by Network! ");
                                    Form1._UI.closeThreads();
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
}
