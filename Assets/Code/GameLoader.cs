using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public void Level1()
    {
        SceneManager.LoadScene("Level_1");
        Debug.Log("Hello, Unity!");
    }
}
