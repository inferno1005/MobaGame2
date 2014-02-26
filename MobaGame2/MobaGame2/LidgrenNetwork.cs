using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace MobaGame2
{
    class LidgrenNetwork
    {
        public NetPeerConfiguration config;
        private NetServer server;
        private NetClient client;
        //private int port = 14242;
        
        public LidgrenNetwork()
        {
            config = new NetPeerConfiguration("MOBA");
            config.Port = 14242;

        }
        public void HostGame()
        {
            Console.WriteLine("HOSTING GAME");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            server = new NetServer(config);
            server.Start();

            NetIncomingMessage inc;

            while ((inc = server.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        //create a response and write some example data to it
                        NetOutgoingMessage response = server.CreateMessage();
                        response.Write("Server Name");

                        //send the response to the sender of the req
                        server.SendDiscoveryResponse(response, inc.SenderEndPoint);
                        break;
                }
            }


        }

        public void FindGame()
        {
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            client = new NetClient(config);
            client.DiscoverLocalPeers(port);
            client.Start();

            Console.WriteLine("trying to find game");
            NetIncomingMessage inc;
            while((inc=client.ReadMessage()) !=null)
            {
                switch(inc.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + inc.SenderEndPoint + "name: " + inc.ReadString());
                        break;
                }
            }
        }

        public void ConnectToHost()
        {
            Console.WriteLine("Trying to connect to host");
            client = new NetClient(config);
            client.Start();
            NetOutgoingMessage outmsg=client.CreateMessage();
            outmsg.Write("TEST");
            client.Connect("192.168.43.147",port,outmsg);
        }


    }
}
