using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Script used for: GameManager

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// Play Music 
    /// </summary>
    /// <param name="audioClip">Music Resources</param>
    public void PlayMusic(AudioClip audioClip) 
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    /// <summary>
    /// Play Sound Effect
    /// </summary>
    /// <param name="audioClip">Music Resources</param>
    public void PlaySound(AudioClip audioClip) 
    {
        audioSource.PlayOneShot(audioClip);
    }

}
