using System.Collections;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This script is taking care of spawning the enemies
/// </summary>
public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float enemySpeed;

    /// <summary>
    /// Whenever someone connects to the server, the coroutine that will start spawning enemies fires up
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(nameof(SpawnEnemies));

    }
    /// <summary>
    /// This function takes care of instantiating a random enemy from at a random position and sets the velocity of the object downwards.
    /// Also it checks the Level, if the score has reached certain amount the level will change, when the level changes the enemies will appear to spawn faster, and to go down faster. The higher the level the faster the enemies
    /// are spawned and the faster they become 
    /// </summary>
    void SpawnEnemy()
    {
        switch (GameManager.instance.lvl)
        {
            case Level.Easy:
                spawnInterval = 3f;
                enemySpeed = 1f;
                break;
            
            case Level.Medium:
                spawnInterval = 2f;
                enemySpeed = 3f;
                break;
            
            case Level.Hard:
                spawnInterval = 1.6f;
                enemySpeed = 4f;
                break;
            
            case Level.Hardcore:
                spawnInterval = 1.3f;
                enemySpeed = 5.6f;
                break;
            
            case Level.Suicide:
                spawnInterval = .7f;
                enemySpeed = 7f;
                break;
        }
        
        if (((ModdedNetworkManager) NetworkManager.singleton).playersList.Count <= 0)
            GameManager.instance.deathScreen.SetActive(true);
        
        Vector2 spawnPosition = new Vector2(Random.Range(-5f, 5f), transform.position.y);
        GameObject enemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPosition, Quaternion.identity);
        enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -enemySpeed);
        NetworkServer.Spawn(enemy);
        Destroy(enemy, 6f);
        
    }


    /// <summary>
    /// This coroutine calls to the function above. I am using Coroutines over the RepeatInvoke function because Coroutines give me more flexibility to stop it at any time and then resume calling it also allows me to adjust
    /// the speed that the enemies are being spawned which is impossible with RepeatInvoke.
    /// This Coroutine has a local variable that starts as true, making this while loop for as long as there are alive players. If there are none, it will change the bool variable causing the whole loop to stop.
    /// Meanwhile it also checks if the game is paused. If the game is paused this Coroutine stops until the game is unpaused. the "()=>" syntax is for anonymous function or lambda expression. This is useful in this scenario
    /// specifically because we don't want it to store anything but just to see if the player has unpaused the game. When the player unpauses the game the coroutine continues
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemies()
    {
        bool arePlayersAlive = true;
        
        while (arePlayersAlive)
        {
            if (Pause.isGamePaused)
            {
                yield return new WaitUntil(()=> !Pause.isGamePaused);
            }
            
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
            
            if (((ModdedNetworkManager) NetworkManager.singleton).playersList.Count <= 0)
            {
                arePlayersAlive = false;
            }
        }
    }
    
}
