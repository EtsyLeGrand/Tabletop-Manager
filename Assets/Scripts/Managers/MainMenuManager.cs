using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnLaunchPlayerScreen() 
    {
        SceneManager.LoadScene("MapScene");
    }

    public void OnOpenMapMenu()
    {

    }

    public void OnOpenMusicMenu()
    {

    }

    public void OnExitApp()
    {
        Application.Quit();
    }
}
