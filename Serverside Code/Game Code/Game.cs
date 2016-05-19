using System;
using System.Collections.Generic;
using PlayerIO.GameLibrary;

namespace FleetServer
{
	public class Player : BasePlayer
    {
        public List<int> deck;
        public List<int> hand;
    }

	[RoomType("1v1")]
	public class GameCode : Game<Player>
    {
        Player player1;
        Player player2;
        Player turnPlayer;

        Ship[][] field;

        List<Ship> scrapheap;

		public override void GameStarted()
        {
			Console.WriteLine("Game is started: " + RoomId);
            PreloadPlayerObjects = true;

            field = new Ship[4][];
            for (int i = 0; i < 4; i++)
                field[i] = new Ship[10];
            scrapheap = new List<Ship>();
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

            if (player1 == null)
                player1 = player;
            else if (player2 == null)
            {
                player2 = player;
                DrawCards(player1, 5);
                DrawCards(player2, 5);
                StartTurn();
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
                case "Attack":
                    Combat(message.GetInt(0), message.GetInt(1), message.GetInt(2), message.GetInt(3));
                    break;
                case "EndTurn":
                    StartTurn();
                    break;
				case "Chat":
					foreach(Player pl in Players) {
						if(pl.ConnectUserId != player.ConnectUserId) {
							pl.Send("Chat", player.ConnectUserId, message.GetString(0));
						}
					}
					break;
			}
		}

        private void StartTurn()
        {
            turnPlayer = (turnPlayer == player1 ? player2 : player1);
            Broadcast("StartTurn", turnPlayer.ConnectUserId);
            DrawCards(turnPlayer);
        }

        private void DrawCards(Player player, int n = 1)
        {
            if (player.deck.Count > 0)
            {
                Broadcast("DrawCard", player.ConnectUserId, n);
                player.hand.Add(player.deck[0]);
                player.deck.RemoveAt(0);
            }
        }

        private void MoveShip(int r1, int t1, int r2, int t2)
        {
            if (field[r1][t1] == null || field[r2][t2] != null || !CheckValidLink(r1, t1, r2, t2))
            {
                Console.Write("Invalid move command");
                return;
            }

            field[r2][t2] = field[r1][t1];
            field[r1][t1] = null;
        }

        private void Combat(int r1, int t1, int r2, int t2)
        {
            if (field[r1][t1] == null || field[r2][t2] == null || !CheckValidLink(r1, t1, r2, t2))
            {
                Console.Write("Invalid attack command");
                return;
            }
            Ship.Battle(field[r1][t1], field[r2][t2]);
            Broadcast("Combat", r1, t1, r2, t2, field[r1][t1].health, field[r1][t1].shield, field[r2][t2].health, field[r2][t2].shield);
            if (field[r1][t1].health <= 0)
            {
                scrapheap.Add(field[r1][t1]);
                field[r1][t1] = null;
            }
            if (field[r2][t2].health <= 0)
            {
                scrapheap.Add(field[r2][t2]);
                field[r2][t2] = null;
            }
        }
        
        /// Check if two tiles are connected, where (r1, t1) corresponds to field[r1][t1]
        private bool CheckValidLink(int r1, int t1, int r2, int t2)
        {
            // use modular arithmetic to account for edge cases
            t1 += 10; t2 += 10;
            int d = (t2 - t1 + 10) % 10;
            // check each condition under which tiles are connected
            if (r1 == r2 && (d == 1 || d == 3))
                return true;
            if (d == 0 && (r1 - r2 == 1 || r1 - r2 == 3))
                return true;
            if ((r1 - r2 == 1 && d == -1) || (r1 - r2 == -1 && d == 1))
                return true;
            return false;
        }
	}
}