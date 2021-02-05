using Mirror;
using UnityEngine;

/// <summary>
/// This script enables the player to shoot, and instantiate bullets 
/// </summary>
public class Shoot : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed;
    
    /// <summary>
    /// On update listens for input but have to check first if the player hasAuthority over the object they own, and if the game is paused. Then a command CmdFire is executed and a sound effect is played.
    /// </summary>
    private void Update()
    {
        if (hasAuthority && Input.GetButtonDown("Fire1") && !Pause.isGamePaused)
        {
            CmdFire();
            SoundManager.soundManager.PlaySoundFX(SoundManager.soundManager.fireFx);
        }
    }
    /// <summary>
    /// Commands are functions that are called only on the server, CmdFire calls DoShoot, which will instantiate a bullet on the current player's screen and by calling RpcShoot it will display the object to the other players as well
    /// without having to network the whole object which causes jitter.
    /// </summary>
    [Command]
    void CmdFire()
    {
        DoShoot();
        RpcShoot();
    }
    
    /// <summary>
    /// Client Rpcs are called on clients only and that will sync data that is passed by the server to all the players
    /// </summary>
    [ClientRpc]
    void RpcShoot()
    {
        // Only call DoShoot on NON-host clients
        if (!isServer) DoShoot();
    }
    
    /// <summary>
    /// Do shoot will only instantiate the object on the player's screen which the rest of the functions will display on the others' screens
    /// </summary>
    void DoShoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * bulletSpeed;
        Destroy(bullet,1f);
    }

    
}