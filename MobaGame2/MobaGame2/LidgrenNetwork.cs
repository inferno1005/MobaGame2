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
        public NetPeerConfiguration sconf;
        public NetPeerConfiguration cconf;
        private NetServer server;
        private NetClient client;
        private int port = 14242;


        public List<AvailableSessions> availsessions;
        public bool isServer = false;
        public bool searching= false;
        
        public LidgrenNetwork()
        {

            availsessions = new List<AvailableSessions>();

            sconf= new NetPeerConfiguration("MOBA");
            sconf.Port = 8080;
            sconf.EnableMessageType(NetIncomingMessageType.DiscoveryRequest );
            sconf.EnableMessageType(NetIncomingMessageType.ConnectionApproval);



            cconf= new NetPeerConfiguration("MOBA");
            cconf.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            //client = new NetClient(config);
        }
        public void HostGame()
        {
            isServer = true;
            Console.WriteLine("HOSTING GAME");
            //sconf.LocalAddress = NetUtility.Resolve("localhost");
            sconf.MaximumConnections = 32;
            server = new NetServer(sconf);
            server.Start();


        
        }
        public void ListenForBroadcast()
        {
            NetIncomingMessage inc;
            while ((inc = server.ReadMessage()) != null)
            {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            Console.WriteLine("got a discovery request");
                            //create a response and write some example data to it
                            NetOutgoingMessage response = server.CreateMessage();
                            response.Write("Remote Game Server!");

                            //send the response to the sender of the req
                            server.SendDiscoveryResponse(response, inc.SenderEndPoint);
                            break;
                        case NetIncomingMessageType.Data:
                            Console.WriteLine("server got data!");
                            break;

                        case NetIncomingMessageType.ConnectionApproval:
                            Console.WriteLine("connection approved");
                            break;
                    }

                    server.Recycle(inc);
            }
        }

        public void FindGame()
        {
            searching = true;
            //client = null;
            client = new NetClient(cconf);

            client.Start();
            client.DiscoverLocalPeers(8080);

            Console.WriteLine("trying to find game");
            
        }
        public void ListenForResponse()
        {
            NetIncomingMessage inc;
            while((inc=client.ReadMessage()) !=null)
            {
                Console.WriteLine("GOT A MESSAGE");
                switch(inc.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:

                        AvailableSessions tempsession=new AvailableSessions();
                        tempsession.ip = inc.SenderEndPoint;
                        tempsession.name=inc.ReadString();
                        availsessions.Add(tempsession);
                        Console.WriteLine("Found server at " + tempsession.ip+ "name: " + tempsession.name);
                        break;

                }
                client.Recycle(inc);
            }
        }

        public void ConnectToHost(System.Net.IPEndPoint ip)
        {
            Console.WriteLine("Trying to connect to host");
            //client = new NetClient(cconf);
            //client.Start();
            NetOutgoingMessage outmsg=client.CreateMessage();
            outmsg.Write("TEST");
            client.Connect(ip,outmsg);
        }

        public void EndSession()
        {
            if(isServer)
                server.Shutdown("shutting down");
            else if(searching)
                client.Shutdown("shutting down");
        }


    }
}
