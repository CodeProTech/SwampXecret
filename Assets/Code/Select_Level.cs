using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select_Level : MonoBehaviour
{
    public void PlayLevel_1()
    {
        SceneManager.LoadSceneAsync(7);
    }

}
