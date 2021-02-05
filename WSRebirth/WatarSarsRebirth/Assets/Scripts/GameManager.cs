using System.Collections;
using Mirror;
using UnityEngine;

/// <summary>
/// This enum holds the game's level
/// </summary>
public enum Level
{
    Easy,
    Medium,
    Hard,
    Hardcore,
    Suicide
}

/// <summary>
/// This script is managing the some of the UI updates. This is singleton to make some of the values more accessible for other scripts
/// </summary>
public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    
    /// <summary>
    /// variable that will check if the player is the host.
    /// </summary>
    public bool isHost;

    /// <summary>
    /// This is used for the joystick that is used to take input from the player once the game is built for android
    /// </summary>
    public Joystick joystick;
    
    public GameObject deathScreen;


    /// <summary>
    /// This level property stores the information from the external classes and updates the SyncVar that is then updated on each client
    /// </summary>
    public Level lvl;
    [SyncVar(hook = nameof(UpdateLvl))] private Level _lvl;

    /// <summary>
    /// On awake the standard Singleton pattern check is performed. If there is an existing instance of this script already running it destroys this instance, else this instance becomes the new GameManager
    /// </summary>
    private void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        
        isHost = isClient && isServer;
    }
    
        
    /// <summary>
    /// When this script is enabled it will assign the values in the NetworkManager so it can display the ui of the players
    /// </summary>
    private void OnEnable()
    {
        ((ModdedNetworkManager) NetworkManager.singleton).mainPanel = UIManager.uiManager.mainPanel;
        ((ModdedNetworkManager) NetworkManager.singleton).playersPanel = UIManager.uiManager.playersPanel;
    }
    

    /// <summary>
    /// OnStartServer is called when an object with NetworkBehaviour becomes active on a server
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(nameof(UpdateLevelStatus)); //This runs the UpdateLevelStatus IEnumerator (Coroutine)
        //InitiateRewards(); //This function initialises the available rewards after the game is done 
    }
    
    /// <summary>
    /// This method is called on the server when the object holding this script becomes unspawned
    /// </summary>
    public override void OnStopServer()
    {
        StopCoroutine(nameof(UpdateLevelStatus)); // Stops the IEnumerator (Coroutine)
        base.OnStopServer();
    }

    /// <summary>
    /// Called on every NetworkBehaviour when a client is active
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();
        
        lvl = ChangeLevel(ScoreManager.instance.totalScore); //the level variable updates based on ChangeLevel() method
        UIManager.uiManager.levelText.text = lvl.ToString(); //Output the level on the UI
    }

    /// <summary>
    /// Hook that will update the SyncVar
    /// </summary>
    /// <param name="_">This parameter is used for the old value of the SyncVar</param>
    /// <param name="newLevel">This parameter is used for the new value of the SyncVar</param>
    void UpdateLvl(Level _, Level newLevel)
    {
        lvl = newLevel; //update the level variable with the new value of the variable
        UIManager.uiManager.levelText.text = lvl.ToString(); //update the text on the UI
    }

    /// <summary>
    /// Changes the level based on the score
    /// </summary>
    /// <param name="score">The current score of the game</param>
    /// <returns></returns>
    public Level ChangeLevel(int score)
    {
        Level changeLevel = Level.Easy;
            
        if(score <= 200)
            changeLevel = Level.Easy;
            
        else if (score <= 500)
            changeLevel = Level.Medium;
            
        else if (score <= 1040)
            changeLevel = Level.Hard;
            
        else if (score <= 3000)
            changeLevel = Level.Hardcore;
            
        else if (score <= 7000)
            changeLevel = Level.Suicide;
        
        return changeLevel;
    }

    /// <summary>
    /// Coroutine that will update the level every 0.1 second
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateLevelStatus()
    {
        
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            lvl = ChangeLevel(ScoreManager.instance.totalScore);
            _lvl = lvl;
            
        }
    }

     /// <summary>
     /// Function that is called when the QuitToMainMenu button is called. It checks whether the player is the host or not and calls the respective function to disconnect the player.
     /// </summary>
    public void ReturnToMainMenu()
    {
        if (isHost)
        {
            ((ModdedNetworkManager)NetworkManager.singleton).QuitToMainMenuAsHost();
        }
        else
        {
            ((ModdedNetworkManager)NetworkManager.singleton).QuitToMainMenuAsClient();
        }
    }

    
}

