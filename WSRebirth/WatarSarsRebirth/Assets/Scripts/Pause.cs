using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool isGamePaused;

    /// <summary>
    /// In update we check if someone has paused the game. This does not affect the server or the other players. The game will be paused only for client that has pressed the pause button and will be updated after they unpause
    /// </summary>
    private void Update()
    {
        PauseGame();
    }

    /// <summary>
    /// This function sets the Timescale to 0 if the isGamePaused is true and to 1 if its false. "Stopping the game using time scale will, effectively, pause every object that is time-based.
    /// This typically accounts for everything that is happening in the game, as movement is usually scaled by delta time, animation, also, is time-based and physics steps will not be called when time scale is zero.
    /// But not everything is stopped, and understanding what’s happening behind the scenes can help to make building a pause system a little bit easier." (John, What does and doesn't get paused, https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/)  
    /// </summary>
    void PauseGame()
    {
        Time.timeScale = isGamePaused ? 0f : 1f;
    }

    /// <summary>
    /// This function will be called whenever the player presses the button. The toggle parameter makes it easier to set in the inspector, by having to check a checkbox
    /// </summary>
    /// <param name="toggled"></param>
    public void TogglePauseMenu(bool toggled)
    {
        isGamePaused = toggled;
    }

}
//This script is a modification of the scripts that can be found here https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/