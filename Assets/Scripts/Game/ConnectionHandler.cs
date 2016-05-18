using PlayerIOClient;
using System.Collections.Generic;

namespace Assets.Scripts.Game
{
    public class ConnectionHandler
    {
        public static Client client;
        public static Connection connection;
        public static GameController game;

        public static void EnterGame(Connection c)
        {
            connection = c;
            c.OnMessage += HandleMessage;
        }

        private static void HandleMessage(object sender, Message e)
        {
            switch (e.Type)
            {
                case "PlayerData":
                    List<int> deck = new List<int>();
                    for (uint i = 0; i < e.Count; i++)
                        deck.Add(e.GetInt(i));
                    game.SetPlayerDeck(deck);
                    break;
                case "OpponentData":
                    break;
                case "Draw":
                    if (e.GetString(0) == client.ConnectUserId)
                        game.PlayerDraw(e.GetInt(1));
                    else
                        game.OpponentDraw(e.GetInt(1));
                    break;
                case "Mill":
                    if (e.GetString(0) == client.ConnectUserId)
                        game.PlayerMill(e.GetInt(1));
                    else
                        game.OpponentMill(e.GetInt(1));
                    break;
                case "Discard":
                    if (e.GetString(0) == client.ConnectUserId)
                        game.PlayerDiscard(e.GetInt(1));
                    else
                        game.OpponentDiscard(e.GetInt(1));
                    break;
                case "Destroy":
                    game.DestroyCard(e.GetInt(0), e.GetInt(1));
                    break;
            }
        }
    }
}
