

using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public int maxHealth;
    public int maxLives;
    
    [SyncVar] private Vector2 _initialStartPosition;
    /// <summary>
    /// the variables above are subject to removal 
    /// </summary>
    
    
    /// <summary>
    /// Events that will be triggered when the respective case is met
    /// </summary>
    public event System.Action<int> OnPlayerNumberChanged; 
    public event System.Action<int> OnPlayerHealthUpdated; 
    public event System.Action<int> OnPlayerLivesUpdated; 
    
    /// <summary>
    /// Prefab that will show up when the player is spawned, and will display the player's stats
    /// </summary>
    [Header("PlayerUIPrefab")] 
    public GameObject playerUIPrefab;
    
    private GameObject _playerUI;

    /// <summary>
    /// SyncVariables that will be synced over the network to all the players
    /// </summary>
    [Header("SyncVars")] 
    
    [SyncVar(hook = nameof(PlayerChangedNumber))]
    public int playerNumber = 0;
    
    [SyncVar(hook = nameof(PlayerHealthUpdated))]
    public int currentHealth;
    
    [SyncVar(hook = nameof(PlayerLivesUpdated))] 
    public int currentLives;

    /// <summary>
    /// Hooks for all the syncVars. The parameters are necessary for the syncVars. This goes for all 3 hook functions
    /// </summary>
    /// <param name="_">old value</param>
    /// <param name="newPlayerNumber">the new value</param>
    void PlayerChangedNumber(int _, int newPlayerNumber)
    {
        OnPlayerNumberChanged?.Invoke(newPlayerNumber);
    }

    void PlayerHealthUpdated(int _, int newCurrentHealth)
    {
        OnPlayerHealthUpdated?.Invoke(newCurrentHealth);
    }

    void PlayerLivesUpdated(int _, int newCurrentLives)
    {
        OnPlayerLivesUpdated?.Invoke(newCurrentLives);
    }

    /// <summary>
    /// When the game starts it will disable the kinematic on the rigidbody on the current player 
    /// </summary>
    private void Start()
    {
        if (isLocalPlayer)
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
    /// <summary>
    /// When the server starts it will add the player to a list to make tracking the number of the alive players easier later on.
    /// Then will set the current health and lives to the maximum amount and will set a variable that will store the position that the player has been spawned at
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();

        //Adding the current player to the list in the NetworkManager
        ((ModdedNetworkManager)NetworkManager.singleton).playersList.Add(this);
        currentHealth = maxHealth;
        currentLives = maxLives;
        _initialStartPosition = transform.position;
    }

    /// <summary>
    /// When the server stops, the player that holds this script is deleted.
    /// CancelInvoke will stop any events from running
    /// </summary>
    public override void OnStopServer()
    {
        CancelInvoke();
        ((ModdedNetworkManager)NetworkManager.singleton).playersList.Remove(this);
    }

    /// <summary>
    /// When a client joins a server, they instantiate the UI and parent it to the ui panel in the game, as well will invoke the events so they can update the
    /// ui of the player. This method will be called on hosts as well, because they act as a server and a client at the same time
    /// </summary>
    public override void OnStartClient()
    {
        _playerUI = Instantiate(playerUIPrefab, ((ModdedNetworkManager) NetworkManager.singleton).playersPanel);

        _playerUI.GetComponent<PlayerUI>().SetPlayer(this, isLocalPlayer);
        
        OnPlayerHealthUpdated.Invoke(currentHealth);
        OnPlayerNumberChanged.Invoke(playerNumber);
        OnPlayerLivesUpdated.Invoke(currentLives);
    }

    /// <summary>
    /// When someone dies, they get unspawned and this function will disable the ui panel for the dead player
    /// </summary>
    public override void OnStopClient()
    {
        Destroy(_playerUI);

        if (isLocalPlayer)
        {
            ((ModdedNetworkManager) NetworkManager.singleton).mainPanel.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// This is for the collision detection in the game. Since all the shooting is done on the server side, its logical to make the collision check only on the server side
    /// that is why this method is marked with the [ServerCallback]. When it detects collision it will play the sound effect, and will check if the player's current health and lives are not 0, if they are greater the enemy will
    /// damage the player but if they are bellow 0 the player will die. Then it will check if the lives are more than 0, which will cause the player to respawn, else the server will unspawn them and will play the death sound
    /// It's noticeable that there are no tag checks, we are using layers. The player and the bullets are set on a layer called Player, and the enemies are set on a layer called Enemies. Then in the physics matrix
    /// they've been set to collide only with each other and nothing else. That way removing the need of checking tags. 
    /// </summary>
    /// <param name="other"></param>
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        SoundManager.soundManager.PlaySoundFX(SoundManager.soundManager.hitFx);

        if (currentLives >= 0 && currentHealth >= 0)
        {
            currentHealth -= other.GetComponent<Enemy>().damage;
        }
        else if (currentLives != 0 && currentHealth <= 0)
        {
            currentLives--;
        	currentHealth = maxHealth;
        	transform.position = _initialStartPosition;
            SoundManager.soundManager.PlaySoundFX(SoundManager.soundManager.deathFX);

        }
        else
        {
            NetworkServer.RemovePlayerForConnection(connectionToClient, true);
        }
    }
}

