using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using PlayerIO.GameLibrary;
using System.Drawing;

namespace FleetServer
{
	public class Player : BasePlayer
    {
		public float posx = 0;
		public float posz = 0;
		public int toadspicked = 0;
	}

	[RoomType("1v1")]
	public class GameCode : Game<Player>
    {
        Player player1;
        Player player2;

		public override void GameStarted()
        {
			Console.WriteLine("Game is started: " + RoomId);
		}

		public override void GameClosed()
        {
			Console.WriteLine("RoomId: " + RoomId);
		}

		public override void UserJoined(Player player)
        {
			foreach(Player pl in Players) {
				if(pl.ConnectUserId != player.ConnectUserId) {
					pl.Send("PlayerJoined", player.ConnectUserId, 0, 0);
					player.Send("PlayerJoined", pl.ConnectUserId, pl.posx, pl.posz);
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