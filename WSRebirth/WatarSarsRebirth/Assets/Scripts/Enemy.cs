using Mirror;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    public int maxHealth;
    [SyncVar] private int _currentHealth;
    public int damage;
    public int dropScore;

    /// <summary>
    /// Whenever an enemy becomes active on the scene OnEnable is called and it sets the current health of the enemy to the max amount
    /// </summary>
    [ServerCallback]
    private void OnEnable()
    {
        _currentHealth = maxHealth;
    }
    
    /// <summary>
    /// This is for the collision detection in the game. Since all the shooting is done on the server side, its logical to make the collision check only on the server side
    /// that is why this method is marked with the [ServerCallback]. When it detects collision it will play the sound effect, and will check if the enemy's current health is not 0, if it is greater the enemy will
    /// continue falling down until they collide with the player. If they do not they get destroyed after 5 seconds. If the enemy collides with an object that holds a Bullet script it means that its been hit by a bullet and
    /// the health will be depleted by the amount of damage that has been set inside the Bullet script. Then it will destroy the gameObject that represents the bullet. This for unknown reason will not work for clients, only for
    /// the host. If the health of the enemy reaches 0, the score variable will be updated, will play a death sound and will destroy the object(enemy) on the server 
    /// </summary>
    /// <param name="other"></param>
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        SoundManager.soundManager.PlaySoundFX(SoundManager.soundManager.hitFx);
        if (other.GetComponent<Bullet>() != null)
        {
            _currentHealth -= other.GetComponent<Bullet>().damage;
            Destroy(other.gameObject);
        }
        
        if (_currentHealth <= 0)
        { 
            ScoreManager.instance.totalScore += dropScore;
            SoundManager.soundManager.PlaySoundFX(SoundManager.soundManager.deathFX);
            NetworkServer.Destroy(gameObject); 
        }
        
    }

    
}