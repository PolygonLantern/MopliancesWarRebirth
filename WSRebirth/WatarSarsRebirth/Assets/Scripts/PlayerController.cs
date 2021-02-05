using System;
using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed;
    
    private Vector2 _screenBounds;
    private float _playerWidth;
    private float _playerHeight;

    /// <summary>
    /// On start of the game when the player is spawned, it will set the variables of screen bounds to the camera's width and height, as well as the player width and height to the sprite's width and height
    /// </summary>
    private void Start()
    {
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        _playerHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        _playerWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    /// <summary>
    /// In the fixed update a check is made to see if the current player owns their object, and to see if the game is paused. The isLocalPlayer check is needed to make sure that the player wont move someone elses character.
    /// Then the player's input is taken from the joystick that is in the scene. Then we change the velocity of the player
    /// </summary>
    private void FixedUpdate()
    {
        if (isLocalPlayer && !Pause.isGamePaused)
        {
            float movement = Input.GetAxis("Horizontal"); //this works for pc 
            
            //float movement = GameManager.instance.joystick.Horizontal;
            GetComponent<Rigidbody2D>().velocity = new Vector2(movement * speed, 0f);
        }
    }
    /// <summary>
    /// In the late update all we do is check if the player's width and height are not going over the boundries of the camera's view
    /// </summary>
    private void LateUpdate()
    {
        Vector3 viewPosition = transform.position;
        viewPosition.x = Mathf.Clamp(viewPosition.x, _screenBounds.x * -1 + _playerWidth, _screenBounds.x - _playerWidth);
        viewPosition.y = Mathf.Clamp(viewPosition.y, _screenBounds.y * -1 + _playerWidth, _screenBounds.y - _playerWidth);
        transform.position = viewPosition;
    }
}
//script used for restricting the player inside the bounds of the camera https://pressstart.vip/tutorials/2018/06/28/41/keep-object-in-bounds.html
//tutorial used for the joystick input https://www.youtube.com/watch?v=bp2PiFC9sSs
//joystick used for the game could be found here https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631
