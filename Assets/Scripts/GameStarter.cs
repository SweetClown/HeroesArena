using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//For the GameStarter

public class GameStarter : MonoBehaviour
{

    public AudioClip audioClip;
    void Start()
    {
        GameManager.Instance.PlayMusic(audioClip);
        Invoke("LoadChoiceCardScene", 1.5f);
    }

    private void LoadChoiceCardScene() 
    {
        SceneManager.LoadScene(1);
    }
}
