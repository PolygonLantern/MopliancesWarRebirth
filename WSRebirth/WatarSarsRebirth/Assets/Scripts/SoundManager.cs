using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
 
    public AudioClip buttonClickFX;
    public AudioClip deathFX;
    public AudioClip hitFx;
    public AudioClip fireFx;
    
    
    /// <summary>
    /// Initialises the singleton variable soundManager and makes it DontDestroyOnLoad so it can seamlessly continue playing the music during the scene changes
    /// </summary>
    private void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Function that will play the passed as parameter sound
    /// </summary>
    /// <param name="soundFX">The sound that we want to play</param>
    
    public void PlaySoundFX(AudioClip soundFX)
    {
        AudioSource.PlayClipAtPoint(soundFX, transform.position);
    }
    
}
