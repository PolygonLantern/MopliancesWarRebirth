using TMPro;
using UnityEngine;
/// <summary>
/// This script is for the UI that is spawned in the players panel, and is used to display the current player's data such as Health, Lives and Name which is represented by a number
/// </summary>
public class PlayerUI : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text playerLivesText;
    public TMP_Text playerHealthText;
    
    private Player _player;
    
    /// <summary>
    /// Whenever a player is spawned this method is called which will subscribe the current player to those events
    /// </summary>
    /// <param name="player"></param>
    /// <param name="isLocalPlayer"></param>
    public void SetPlayer(Player player, bool isLocalPlayer)
    {
        this._player = player;

        player.OnPlayerNumberChanged += OnPlayerNumberChanged;
        player.OnPlayerHealthUpdated += OnPlayerHealthUpdated;
        player.OnPlayerLivesUpdated += OnPlayerLivesUpdated;

    }
    /// <summary>
    /// As a thumb rule whenever you subscribe to an event you also have to unsubscribe when you disable or destroy the object to prevent a memory leak
    /// </summary>
    private void OnDisable()
    {
        _player.OnPlayerNumberChanged -= OnPlayerNumberChanged;
        _player.OnPlayerNumberChanged -= OnPlayerHealthUpdated;
        _player.OnPlayerNumberChanged -= OnPlayerLivesUpdated;
    }

    /// <summary>
    /// Hooks for the events
    /// </summary>
    /// <param name="newPlayerNumber"></param>
    void OnPlayerNumberChanged(int newPlayerNumber)
    {
        playerNameText.text = newPlayerNumber.ToString();
    }

    void OnPlayerHealthUpdated(int newHealthValue)
    {
        playerHealthText.text = newHealthValue.ToString();
    }

    void OnPlayerLivesUpdated(int newLivesValue)
    {
        playerLivesText.text = newLivesValue.ToString();
    }
}
//This script enhanced version of Mirror's Basic Example. PlayerUI script;
