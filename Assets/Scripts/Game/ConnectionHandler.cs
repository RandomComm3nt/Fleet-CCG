using PlayerIOClient;

namespace Assets.Scripts.Game
{
    public class ConnectionHandler
    {
        public static Client client;
        public static Connection connection;
        public static Field field;

        public static void EnterGame(Connection c)
        {
            connection = c;
            c.OnMessage += HandleMessage;
        }

        private static void HandleMessage(object sender, Message e)
        {
            
        }
    }
}
