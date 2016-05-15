using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerIOClient;
using Assets.Scripts.Game;
using System.Collections.Generic;

namespace Assets.Scripts.Menus
{
    public class Lobby : MonoBehaviour
    {
  
        private void Start()
        {
            ConnectionHandler.client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);
            ConnectionHandler.client.Multiplayer.ListRooms("1v1", null, 25, 0, new Callback<RoomInfo[]>(HandleRooms), new Callback<PlayerIOError>(HandleError));
        }
        
        private void Update()
        {

        }

        public void CreateGame()
        {
            ConnectionHandler.client.Multiplayer.CreateJoinRoom("Room", "1v1", true, null, null, new Callback<Connection>(HandleConnect), new Callback<PlayerIOError>(HandleError));
        }

        private void HandleRooms(RoomInfo[] rooms)
        {

        }

        private void HandleConnect(Connection connection)
        {
            Debug.Log("Created");
            ConnectionHandler.EnterGame(connection);
            SceneManager.LoadScene("Game");
        }

        private void HandleError(PlayerIOError error)
        {
            Debug.Log(error);
        }
    }
}