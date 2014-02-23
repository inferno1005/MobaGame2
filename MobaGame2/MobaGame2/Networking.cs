using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;



namespace MobaGame2
{
    class Networking
    {
        public enum PacketType { Enter, Leave, Data}


        public NetworkSession networkSession;
        public AvailableNetworkSessionCollection availableSessions;
        public int selectedSessionIndex;
        public PacketReader packetReader = new PacketReader();
        public PacketWriter packetWriter = new PacketWriter();
        bool isServer = false;


        public void SignedInGamer_SignedIn(object sender, SignedInEventArgs e)
        {
            e.Gamer.Tag = new Player();
        }
        public void CreateSession()
        {
            if(networkSession!=null)
                networkSession.Dispose();
            networkSession = NetworkSession.Create(NetworkSessionType.SystemLink, 1, 10, 2, null);

            if (networkSession == null)
                Console.WriteLine("failed to create session");

            networkSession.AllowHostMigration = true;
            networkSession.AllowJoinInProgress= true;

            AddNetworkingEvents();
        }
        public void EndSession()
        {
            networkSession.Dispose();
        }
        public void SendPackets(PacketType packetType)
        {
            switch (packetType)
            {
                case PacketType.Data:
                    {
                        foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
                        {
                            //this will write stuff
                            packetWriter.Write(Vector2.Zero);
                            gamer.SendData(packetWriter, SendDataOptions.ReliableInOrder);
                        }


                    break;
                    }
            }
        }
        public void RecievePackets()
        {
            NetworkGamer sender;
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                while (gamer.IsDataAvailable)
                {
                    gamer.ReceiveData(packetReader, out sender);

                    if (!gamer.IsHost)
                    {
                        //read data and apply

                    }

                }
            }
        }

        public static void SignIn()
        {
            if (!Guide.IsVisible)
            {
                Guide.ShowSignIn(1, false);
            }
        }

        protected void AddNetworkingEvents()
        {
            networkSession.GamerJoined += new EventHandler<GamerJoinedEventArgs>(networkSession_GamerJoined);
            networkSession.GamerLeft+= new EventHandler<GamerLeftEventArgs>(networkSession_GamerLeft);
            networkSession.GameStarted+= new EventHandler<GameStartedEventArgs>(networkSession_GameStarted);
            networkSession.GameEnded+= new EventHandler<GameEndedEventArgs>(networkSession_GameEnded);
            networkSession.SessionEnded+= new EventHandler<NetworkSessionEndedEventArgs>(networkSession_SessionEnded);
        }

        protected void RemoveNetworkingEvents()
        {

            networkSession.GamerJoined -= new EventHandler<GamerJoinedEventArgs>(networkSession_GamerJoined);
            networkSession.GamerLeft-= new EventHandler<GamerLeftEventArgs>(networkSession_GamerLeft);
            networkSession.GameStarted-= new EventHandler<GameStartedEventArgs>(networkSession_GameStarted);
            networkSession.GameEnded-= new EventHandler<GameEndedEventArgs>(networkSession_GameEnded);
            networkSession.SessionEnded-= new EventHandler<NetworkSessionEndedEventArgs>(networkSession_SessionEnded);
        }

        public void Update()
        {
            if(networkSession!=null)
            {
                networkSession.Update();
            }
        }


        //private void HookSessionEvents()
        //{
            //networkSession.GamerJoined += new EventHandler<GamerJoinedEventArgs>(networkSession_GamerJoined);
        //}
        private void networkSession_GamerJoined(object sender, GamerJoinedEventArgs e)
        {
            if (!e.Gamer.IsLocal)
            {
                e.Gamer.Tag = new Player();
            }
            else
            {
                e.Gamer.Tag = GetPlayer(e.Gamer.Gamertag);
            }
        }
        private void networkSession_GamerLeft(object sender, GamerLeftEventArgs e)
        {
        }
        private void networkSession_GameStarted(object sender, GameStartedEventArgs e)
        {
        }
        private void networkSession_GameEnded(object sender, GameEndedEventArgs e)
        {
        }
        private void networkSession_SessionEnded(object sender, NetworkSessionEndedEventArgs e)
        {
        }
        private Player GetPlayer(String gamertag)
        {
            foreach (SignedInGamer signedInGamer in SignedInGamer.SignedInGamers)
            {
                if (signedInGamer.Gamertag == gamertag)
                {
                    return signedInGamer.Tag as Player;
                }
            }
            return new Player();
        }
    }
}
