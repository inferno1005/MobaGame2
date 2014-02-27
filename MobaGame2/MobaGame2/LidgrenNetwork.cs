using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace MobaGame2
{
    public struct AvailableSessions
    {
        public System.Net.IPEndPoint ip;
        public string name;
    }

    class LidgrenNetwork
    {
        public NetPeerConfiguration sconf;      //server config
        public NetPeerConfiguration cconf;      //client config

        private NetServer server;
        private NetClient client;


        private int port ;               //port for server to use
        private string GameName;


        public List<AvailableSessions> availsessions;
        public List<System.Net.IPEndPoint> clients;
        public string ServerName;
        public bool isServer = false;
        public bool searching= false;
        public bool joined = false;
        
        public LidgrenNetwork()
        {
            //server specific code
            ServerName = "Game server";
            port = 8080;
            GameName = "MOBA";
        }
        public void HostGame()
        {
            sconf= new NetPeerConfiguration(GameName);
            sconf.Port = port;
            sconf.EnableMessageType(NetIncomingMessageType.DiscoveryRequest );
            sconf.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            isServer = true;
            Console.WriteLine("HOSTING GAME");

            sconf.MaximumConnections = 32;
            server = new NetServer(sconf);


            clients = new List<System.Net.IPEndPoint>();

            server.Start();


        
        }
        public void ListenMessage()
        {
            NetIncomingMessage inc;
            if (isServer)   //do server listening stuff
            {
                while ((inc = server.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            Console.WriteLine("got a discovery request");
                            //create a response and write some example data to it
                            NetOutgoingMessage response = server.CreateMessage();
                            response.Write(ServerName);

                            //send the response to the sender of the req
                            server.SendDiscoveryResponse(response, inc.SenderEndPoint);
                            break;
                        case NetIncomingMessageType.Data:
                            Console.WriteLine("server got data!");
                            break;

                        case NetIncomingMessageType.ConnectionApproval:
                            clients.Add(inc.SenderEndPoint);
                            Console.WriteLine("connection approved");
                            //need to send world state from server to client
                            break;
                    }

                    server.Recycle(inc);
                }
            }
            //client stuff
            else
            {
                while ((inc = client.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {

                        //response back from server after searching for a server
                        case NetIncomingMessageType.DiscoveryResponse:

                            AvailableSessions tempsession = new AvailableSessions();
                            tempsession.ip = inc.SenderEndPoint;
                            tempsession.name = inc.ReadString();
                            availsessions.Add(tempsession);
                            Console.WriteLine("Found server at " + tempsession.ip + "name: " + tempsession.name);
                            break;



                    }

                    client.Recycle(inc);
                }
            }
        }

        //search the subnet for games 
        public void FindGame()
        {
            availsessions = new List<AvailableSessions>();

            cconf = new NetPeerConfiguration(GameName);
            cconf.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            searching = true;
            client = new NetClient(cconf);

            client.Start();
            client.DiscoverLocalPeers(port);
        }


        //client connect to known ip:port
        public void ConnectToHost(System.Net.IPEndPoint ip)
        {
            Console.WriteLine("Trying to connect to host @ {0}" ,ip.ToString());
            NetOutgoingMessage outmsg=client.CreateMessage();
            outmsg.Write("Connecting");
            client.Connect(ip,outmsg);
        }

        public void EndSession()
        {
            if (isServer)
            {
                isServer = false;
                server.Shutdown("shutting down");
                server = null;
            }
            else if (searching)
            {
                searching= false;
                client.Shutdown("shutting down");
                client = null;
            }
            availsessions = null;
        }


    }
}
