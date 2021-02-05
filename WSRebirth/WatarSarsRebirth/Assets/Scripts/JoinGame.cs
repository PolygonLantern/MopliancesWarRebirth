using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This script handles the connecting a player to a host
/// </summary>
public class JoinGame : NetworkBehaviour
{
   private ModdedNetworkManager _networkManager;

   [SerializeField] private TMP_InputField ipInputField;
   [SerializeField] private Button joinGameButton;
   
   /// <summary>
   /// Whenever the object that holds the script is enabled it will find the NetworkManager, and will also subscribe the current client to the OnConnected and OnDisconnected events that are in the NetworkManager
   /// </summary>
   private void OnEnable()
   {
      _networkManager = FindObjectOfType<ModdedNetworkManager>();

      ModdedNetworkManager.OnClientConnected += HandleClientConnected;
      ModdedNetworkManager.OnClientDisconnected += HandleClientDisconnected;
   }

   /// <summary>
   /// Whenever we disable this panel, i.e when the player quits the game we unsubscribe from those events, to prevent memory leak
   /// </summary>
   private void OnDisable()
   {
      ModdedNetworkManager.OnClientConnected -= HandleClientConnected;
      ModdedNetworkManager.OnClientDisconnected -= HandleClientDisconnected;
   }

   /// <summary>
   /// This function will be called whenever the join game button is pressed. It will take the ip that has been passed in the input field, then will assign it to the NetworkManager and will ask it to connect to the ip.
   /// Then we disable the button to prevent spamming the join button.
   /// </summary>
   public void Join()
   {
      string ipAddress = ipInputField.text;

      _networkManager.networkAddress = ipAddress;
      _networkManager.StartClient();
      
      joinGameButton.interactable = false;
   }
   
   /// <summary>
   /// Hooks for the events. When a player is connected to a server, we make the button interactable again so when they disconnect to be able to connect again. Then we set the panel to off so when they disconnect from the game
   /// they will see the main menu, not the last panel that they've been before they connected to the game.
   /// </summary>
   private void HandleClientConnected()
   {
      joinGameButton.interactable = true;
      gameObject.SetActive(false);
      
   }
   /// <summary>
   /// If for whatever reason they fail to connect, we set the button to interactable so they can try again
   /// </summary>
   private void HandleClientDisconnected()
   {
      joinGameButton.interactable = true;
   }
}

//Code from DapperDino's tutorial - https://www.youtube.com/watch?v=Fx8efi2MNz0&t=770s
