﻿//todo 
//add connection handshake, doesnt seem reliable,if the packet is dropped

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Lidgren.Network;
using Lidgren.Network.Xna;


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
        public string ServerName;
        public bool isServer = false;
        public bool searching= false;
        public bool joined = false;
        public bool GameIsRunning = false;
        public bool inLobby = false;
        
        public LidgrenNetwork()
        {
            //server specific code
            ServerName = "Game server";
            port = 8080;
            GameName = "MOBA";
        }
        public void HostGame()
        {
            //setup server config
            sconf= new NetPeerConfiguration(GameName);
            sconf.Port = port;
            sconf.EnableMessageType(NetIncomingMessageType.DiscoveryRequest );
            sconf.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            sconf.EnableMessageType(NetIncomingMessageType.Data);

            isServer = true;
            Console.WriteLine("HOSTING GAME");

            sconf.MaximumConnections = 32;
            server = new NetServer(sconf);

            server.Start();


        
        }
        public object ListenMessage()
        {
            NetIncomingMessage inc;
            if (isServer)   //do server listening stuff
            {
                while ((inc = server.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                            //listens for a peer discovery request
                        case NetIncomingMessageType.DiscoveryRequest:
                            Console.WriteLine("got a discovery request");

                            //if game is running, dont bother sending a discovery response
                            if(!GameIsRunning)
                            {
                                //create a message
                                NetOutgoingMessage response = server.CreateMessage();
                                response.Write(ServerName);

                                //send the response to the sender of the req
                                server.SendDiscoveryResponse(response, inc.SenderEndPoint);
                            }
                            break;
                            //listens for data
                        case NetIncomingMessageType.Data:


                            object temp;

                            //deserialize the data
                            temp = DeserializeObject<object>(inc.Data);

                            //if its player info return
                            if (temp is Player)
                            {
                                return temp;
                            }

                            //if its ability info return
                            else if (temp is Ability)
                            {
                                return temp;
                            }

                            break;

                            //if a client is wanting to join send response
                        case NetIncomingMessageType.ConnectionApproval:
                            //if game is already running, do not let new people join
                            if (!GameIsRunning)
                            {
                                inc.SenderConnection.Approve();
                                NetOutgoingMessage msg = server.CreateMessage();
                                Console.WriteLine("connection from {0} approved", inc.SenderEndPoint);
                            }

                            else
                            {
                                inc.SenderConnection.Deny();
                                Console.WriteLine("connection from {0} denied", inc.SenderEndPoint);
                            }
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


                        case NetIncomingMessageType.Data:



                            object temp;
                            temp = DeserializeObject<object>(inc.Data);


                            //if getting gamestate
                            if (temp is GameState)
                            {
                                return temp;
                            }
                            else if (temp is Player)
                            {
                                return temp;
                            }

                        //string
                            else if (temp is string)
                            {
                                switch ((string)temp)
                                {
                                    case "start game":
                                        GameIsRunning = true;
                                        inLobby = false;
                                        break;
                                    case "end game":
                                        inLobby = false;
                                        GameIsRunning = false;
                                        break;
                                }
                            }

                            //get client index for players
                            else if (temp is int)
                            {
                                return temp;
                            }
                            break;


                            //if the status of the client changes
                        case NetIncomingMessageType.StatusChanged:
                            switch (client.ConnectionStatus)
                            {
                                    //if connected to server
                                case NetConnectionStatus.Connected:
                                    inLobby = true;
                                    GameIsRunning = false;
                                    isServer = false;
                                    Console.WriteLine("connected");
                                    break;
                                    //if disconnected from server
                                case NetConnectionStatus.Disconnected:
                                    Console.WriteLine("disconnected");
                                    break;

                                case NetConnectionStatus.RespondedConnect:
                                    Console.WriteLine("responded connect");
                                    break;

                                case NetConnectionStatus.Disconnecting:
                                    Console.WriteLine("disconnecting");
                                    break;

                                case NetConnectionStatus.InitiatedConnect:
                                    Console.WriteLine("initiated connect");
                                    break;

                                case NetConnectionStatus.None:
                                    Console.WriteLine("no status change");
                                    break;

                                case NetConnectionStatus.ReceivedInitiation:
                                    Console.WriteLine("Received Initiation");
                                    break;

                                case NetConnectionStatus.RespondedAwaitingApproval:
                                    Console.WriteLine("Responded Awaiting Approval");
                                    break;
                            }
                            break;
                    }

                    client.Recycle(inc);
                }

            }
            return null;
        }

        //search the subnet for games 
        public void FindGame()
        {
            availsessions = new List<AvailableSessions>();

            cconf = new NetPeerConfiguration(GameName);
            cconf.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            cconf.EnableMessageType(NetIncomingMessageType.Data);
            cconf.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

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



            searching = false;
            inLobby = true;
        }

        //client connect to known ip:port
        //this was used for testing across the internet
        public void ConnectToIP(string IP)
        {
            cconf = new NetPeerConfiguration(GameName);
            cconf.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            cconf.EnableMessageType(NetIncomingMessageType.Data);

            client = new NetClient(cconf);

            client.Start();

            //client.Connect("192.168.1.118", 8080);  
            client.Connect(IP, 8080);  


            searching = false;
            inLobby = true;
        }



        public void SendObject(object Object)
        {
            NetOutgoingMessage sendMsg;
            if (isServer)
                sendMsg=server.CreateMessage();
            else
                sendMsg=client.CreateMessage();

            sendMsg.Write(SerializeObject(Object));

            //if server 
            if (isServer)
            {
                //and we have clients
                if (server.ConnectionsCount > 0)
                { 
                    //send message to all clients
                    //unreliable, UDP, faster, fine on lan
                    server.SendMessage(sendMsg, server.Connections, NetDeliveryMethod.Unreliable, 0);
                }
            }
            else
            {
                //send to server
                client.SendMessage(sendMsg, NetDeliveryMethod.Unreliable,0);
            }

        }


        //takes any object and converts to binary stream
        private static byte[] SerializeObject(object pObjectToSerialize)
        {
            BinaryFormatter lFormatter = new BinaryFormatter();
            MemoryStream lStream = new MemoryStream();
            lFormatter.Serialize(lStream, pObjectToSerialize);
            byte[] lRet = new byte[lStream.Length];
            lStream.Position = 0;
            lStream.Read(lRet, 0, (int)lStream.Length);
            lStream.Close();
            return lRet;
        }

        //takes any binary stream and converts back to an object
        public static T DeserializeObject<T>(byte[] pData)
        {
            if (pData == null)
                return default(T);
            BinaryFormatter lFormatter = new BinaryFormatter();
            MemoryStream lStream = new MemoryStream(pData);
            object lRet = lFormatter.Deserialize(lStream);
            lStream.Close();
            return (T)lRet;
        }

        //shut down the network session
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

        //return the number of players on the server
        public int PlayerCount()
        {
            return server.ConnectionsCount+1; 
        }


    }
}
