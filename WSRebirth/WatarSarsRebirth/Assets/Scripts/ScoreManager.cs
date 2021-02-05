using Mirror;
using TMPro;

public class ScoreManager : NetworkBehaviour
{
    // assign in inspector if this script is on the scene canvas
    public TMP_Text scoreText;

    public static ScoreManager instance;

    /// <summary>
    /// Check if there are not other instances of this class
    /// </summary>
    void Start()
    {
        if (instance == null) instance = this;
    }

    /// <summary>
    /// Set up a hook variable that will be used to store the score.
    /// SyncVars are synchronised amongst all of the clients as well as the host.
    /// </summary>
    [SyncVar(hook = nameof(SetScore))]
    public int totalScore;
    
    /// <summary>
    /// Hook for the syncVar. This will just update the text once the value of the syncVar is changed
    /// </summary>
    /// <param name="oldScore"></param>
    /// <param name="newScore"></param>
    void SetScore(int oldScore, int newScore)
    {
        scoreText.text = totalScore.ToString();
    }

}