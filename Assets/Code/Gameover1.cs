using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    private Health healthScript;

    void Start()
    {
        // Finde das Health-Skript in der Szene
        healthScript = FindObjectOfType<Health>();
    }

    void Update()
    {
        // Update-Logik, falls ben�tigt
    }

    public void Restart()
    {
        if (healthScript != null)
        {
            healthScript.ResetHealth(); // Setze den Gesundheitsstatus zur�ck
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WatchAdToContinue()
    {
        // Hier Unity Ads laden und Leben auff�llen
        // Gameover Screen weg machen
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

