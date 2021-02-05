using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private ModdedNetworkManager _networkManager;
    
    /// <summary>
    /// Whenever the game object that becomes active on the scene. The game will find the networkManager 
    /// </summary>
    private void OnEnable()
    {
        if (_networkManager == null)
        {
            _networkManager = FindObjectOfType<ModdedNetworkManager>();
        }
    }

    /// <summary>
    /// Function that will be called on the start as host button
    /// </summary>
    public void StartHost()
    {
        _networkManager.StartHost();
    }

    /// <summary>
    /// Function that will be called on the Exit button
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    
    
}
