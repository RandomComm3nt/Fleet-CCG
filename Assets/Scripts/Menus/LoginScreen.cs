using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayerIOClient;

namespace Assets.Scripts.Menus
{
    public class LoginScreen : MonoBehaviour
    {
        [SerializeField] private InputField usernameField;
        [SerializeField] private InputField passwordField;
        [SerializeField] private Text responseText;
        [SerializeField] private Dropdown skipDropdown;

        private void Start ()
        {
	    }
	
	    public void Login()
        {
            responseText.text = "Attempting login...";
            PlayerIO.QuickConnect.SimpleConnect("fleet-ccg-ocd1c5auhearosikjgk3bw", usernameField.text, passwordField.text, null, new Callback<Client>(SuccessHandler), new Callback<PlayerIOError>(ErrorHandler));
        }

        public void Register()
        {
            responseText.text = "Attempting registration...";
            PlayerIO.QuickConnect.SimpleRegister("fleet-ccg-ocd1c5auhearosikjgk3bw", usernameField.text, passwordField.text, null, null, null, null, null, null, new Callback<Client>(SuccessHandler), new Callback<PlayerIORegistrationError>(ErrorHandler));
        }

        private void SuccessHandler(Client client)
        {
            Game.ConnectionHandler.client = client;
            SceneManager.LoadScene("Lobby");   
        }

        private void ErrorHandler(PlayerIOError error)
        {
            responseText.text = error.Message;
        }

        public void SkipLogin()
        {
            usernameField.text = "TestAccount-" + skipDropdown.options[skipDropdown.value].text;
            passwordField.text = "Testing123";
            Login();
        }
}
}