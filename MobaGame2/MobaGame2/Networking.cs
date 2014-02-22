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



namespace MobaGame2
{
    static class Networking
    {
        public static  NetworkSession networkSession;
        public  static AvailableNetworkSessionCollection availableSessions;
        public  static int selectedSessionIndex;
        public  static PacketReader packetReader = new PacketReader();
        public  static PacketWriter packetWriter = new PacketWriter();

        public static void SignedInGamer_SignedIn(object sender, SignedInEventArgs e)
        {
            e.Gamer.Tag = new Player();
        }
        public static void CreateSession()
        {
            networkSession = NetworkSession.Create(NetworkSessionType.SystemLink, 1, 10, 2, null);

            networkSession.AllowHostMigration = true;
            networkSession.AllowJoinInProgress= true;

            HookSessionEvents();
        }
        private static void HookSessionEvents()
        {
            networkSession.GamerJoined += new EventHandler<GamerJoinedEventArgs>(networkSession_GamerJoined);
        }
        private static void networkSession_GamerJoined(object sender, GamerJoinedEventArgs e)
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
        private static Player GetPlayer(String gamertag)
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
