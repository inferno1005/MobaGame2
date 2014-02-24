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
        public enum GameMode { AllMiddle }
        public enum SkillLevel{ Beginner,Intermediate,Advanced}
        public enum SessionProperty{GameMode,SkillLevel,ScoreToWin}


        public NetworkSession networkSession { get; private set; }
        public AvailableNetworkSessionCollection availableSessions;
        public AvailableNetworkSession availableSession;
        public int selectedSessionIndex;
        public PacketReader packetReader = new PacketReader();
        public PacketWriter packetWriter = new PacketWriter();
        public NetworkSessionState networkSessionState;
        public bool isServer = false;


        public void SignedInGamer_SignedIn(object sender, SignedInEventArgs e)
        {
            e.Gamer.Tag = new Player();
        }
/*
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
*/
        public void EndSession()
        {
            if (networkSession != null)
            {
                networkSession.Dispose();
                networkSession = null;
            }
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
        public void SignIn()
       {
            if (!Guide.IsVisible)
            {
                Guide.ShowSignIn(1, false);
            }
        }
        public void HostGame()
        {
            if (SignedInGamer.SignedInGamers.Count == 0)
                SignIn();
            else if (SignedInGamer.SignedInGamers.Count == 1)
            {
                NetworkSessionProperties sessionProperties = new NetworkSessionProperties();

                sessionProperties[(int)SessionProperty.GameMode] = (int)GameMode.AllMiddle;

                sessionProperties[(int)SessionProperty.SkillLevel] = (int)SkillLevel.Beginner;

                sessionProperties[(int)SessionProperty.ScoreToWin] = 1; //kill base to win

                int maximumGamers = 10;
                int privateGamerSlots = 1;
                int maximumLocalPlayers = 1;


                networkSession= NetworkSession.Create(
                        NetworkSessionType.SystemLink,
                        maximumLocalPlayers,
                        maximumGamers,
                        privateGamerSlots,
                        sessionProperties);


                isServer = true;
                networkSession.AllowHostMigration = true;
                networkSession.AllowJoinInProgress= true;
                networkSessionState = NetworkSessionState.Lobby;

                //gameState= GameState.PlayGame;
            }

        }
        public void FindGame()
        {
            if (SignedInGamer.SignedInGamers.Count == 0)
                SignIn();
            else if (SignedInGamer.SignedInGamers.Count == 1)
            {
                //gameState = GametState.FindeGame;

                int maximumLocalPlayers = 1;

                NetworkSessionProperties searchProperties = new NetworkSessionProperties();

                searchProperties[(int)SessionProperty.GameMode] = (int)GameMode.AllMiddle;
                searchProperties[(int)SessionProperty.SkillLevel] = (int)SkillLevel.Beginner;

                availableSessions = NetworkSession.Find(
                    NetworkSessionType.SystemLink,
                    maximumLocalPlayers,
                    searchProperties);

                if (availableSessions.Count != 0)
                {
                    availableSession = availableSessions[selectedSessionIndex];
                }
                isServer = false;

            }
        }
        public void JoinGame()
        {
            if(availableSessions.Count -1 >=selectedSessionIndex)
            {
                networkSession = NetworkSession.Join(availableSessions[selectedSessionIndex]);
                networkSessionState = NetworkSessionState.Lobby;

                AddNetworkingEvents();

                //gameState = GameState.PlayGame;
            }

        }
        public void Update()
        {
            if(networkSession!=null)
            {
                networkSession.Update();
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
