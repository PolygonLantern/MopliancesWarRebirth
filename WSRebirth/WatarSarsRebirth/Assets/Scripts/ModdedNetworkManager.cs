using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ModdedNetworkManager : NetworkManager
{   /// <summary>
    /// A list that is marked as internal which will make sure that there is only one list that is called playersList in the whole assembly build. This lists is just to keep track of players that are alive.
    /// </summary>
    internal readonly List<Player> playersList = new List<Player>();
    
    public RectTransform mainPanel;
    public RectTransform playersPanel;
    
    /// <summary>
    /// Events that will fire upon player connecting or disconnecting
    /// </summary>
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    /// <summary>
    /// Whenever a player(client) connects to a server, the server will check if the mainPanel and the playersPanel are assigned and if they are not, they will get a reference from the UI Manager
    /// Then the OnClientConnected event will be registered. 
    /// </summary>
    /// <param name="conn"></param>
     public override void OnClientConnect(NetworkConnection conn)
    {
      base.OnClientConnect(conn);
      
      if(mainPanel == null) mainPanel = UIManager.uiManager.mainPanel;
      if(playersPanel == null) playersPanel = UIManager.uiManager.playersPanel;
      
      OnClientConnected?.Invoke();
    }

    /// <summary>
    /// Whenever a player disconnects the event will be registered
    /// </summary>
    /// <param name="conn"></param>
     public override void OnClientDisconnect(NetworkConnection conn)
     {
         base.OnClientDisconnect(conn);
         OnClientDisconnected?.Invoke();
     }

    /// <summary>
    /// Whenever someone joins the server, the on server connect is called. The check for the main and players panel is in case this is called on the Host that acts as a server and a client. Then we check if the number
    /// of connected players are not exceeding the maximum that is allowed (Which is 4 for this game). If it does it will disconnect the player that is trying to join.
    /// </summary>
    /// <param name="conn"></param>
     public override void OnServerConnect(NetworkConnection conn)
    {
      base.OnServerConnect(conn);
      
      if(mainPanel == null) mainPanel = UIManager.uiManager.mainPanel;
      if(playersPanel == null) playersPanel = UIManager.uiManager.playersPanel;

      if (numPlayers > maxConnections)
      {
          conn.Disconnect();
          return;
      }
      
    }

    /// <summary>
    /// Whenever a player successfully connects to the server this function is called. We add that player to the list and update a number value that will be the player's name
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        ResetPlayerNumbers();
    }
    
    /// <summary>
    /// This is called whenever a player disconnects and refreshes the players number
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        ResetPlayerNumbers();
    }

    /// <summary>
    /// This function is called whenever a button is pressed, and the player that is calling to it is a client (just a player). StopClient will disconnect the player that has pressed the button.
    /// </summary>
    public void QuitToMainMenuAsClient()
    {
        StopClient();    
    }
    
    /// <summary>
    /// This function is called whenever a button is pressed and the player is the host. StopHost will automatically kick all of the players that are not the host and will disconnect the host.
    /// </summary>
    public void QuitToMainMenuAsHost()
    {
        StopHost();
    }
    
    /// <summary>
    /// This method loops through the playersList, that is updated whenever a player connects or disconnects. Then sets a local variable that will act as a counter for how many players are in the list, a.k.a
    /// how many players have joined the game. Then it will pass the value to the player's playerNumber that is displayed in the player's name 
    /// </summary>
    void ResetPlayerNumbers()
    {
        int playerNumber = 0;
        foreach (Player player in playersList)
        {
            player.playerNumber = playerNumber;
            playerNumber++;
        }
    }
}
//This script is modified version of the BasicNetworkManager in Mirror's Basic example