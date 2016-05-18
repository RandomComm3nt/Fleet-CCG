using System;
using System.Collections.Generic;
using PlayerIO.GameLibrary;

namespace FleetServer
{
	public class Player : BasePlayer
    {
        public List<int> deck;
	}

	[RoomType("1v1")]
	public class GameCode : Game<Player>
    {
        Player player1;
        Player player2;

		public override void GameStarted()
        {
			Console.WriteLine("Game is started: " + RoomId);
            PreloadPlayerObjects = true;
		}

		public override void GameClosed()
        {
			Console.WriteLine("RoomId: " + RoomId);
		}

		public override void UserJoined(Player player)
        {
            if (!player.PlayerObject.Contains("Decks"))
            {
                player.PlayerObject.Set("Decks", new DatabaseArray());
            }
            if (player.PlayerObject.GetArray("Decks").Count == 0)
            {
                DatabaseObject o = new DatabaseObject();
                DatabaseArray a = new DatabaseArray();
                for (int i = 0; i < 30; i++)
                {
                    a.Add(i);
                }
                o.Set("Cards", a);
                player.PlayerObject.GetArray("Decks").Add(o);
                player.PlayerObject.Save();
            }

            DatabaseArray cards = player.PlayerObject.GetArray("Decks").GetObject(0).GetArray("Cards");
            player.deck = new List<int>();
            Message m = Message.Create("PlayerData");
            for (int i = 0; i < cards.Count; i++)
            {
                player.deck.Add(cards.GetInt(i));
                m.Add(player.deck[i]);
            }
            player.Send(m);

			foreach(Player pl in Players)
            {
				if(pl.ConnectUserId != player.ConnectUserId)
                {
                    pl.Send("OpponentData", player.ConnectUserId, player.deck.Count);
				}
			}
		}

		public override void UserLeft(Player player)
        {
			Broadcast("PlayerLeft", player.ConnectUserId);
		}

		public override void GotMessage(Player player, Message message)
        {
			switch(message.Type)
            {
				
				case "Chat":
					foreach(Player pl in Players) {
						if(pl.ConnectUserId != player.ConnectUserId) {
							pl.Send("Chat", player.ConnectUserId, message.GetString(0));
						}
					}
					break;
			}
		}
	}
}