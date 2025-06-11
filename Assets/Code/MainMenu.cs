using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayMainMenu() // fuert ins MainMenue
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void PlayGame() // fuert ins MainMenue
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void PlayEndless()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayLevel()
    {
        SceneManager.LoadSceneAsync(9);
    }

    public void PLayLevel2()
    {
        SceneManager.LoadSceneAsync(11);
    }

    public void PlayStory()
    {
        SceneManager.LoadSceneAsync(4);
    }

    public void Settings()
    {
        SceneManager.LoadSceneAsync(5);
    }
    public void LoadSelectLevel()
    {
        SceneManager.LoadSceneAsync(7);
    }
}
