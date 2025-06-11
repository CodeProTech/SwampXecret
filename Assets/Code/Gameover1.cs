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
        // Update-Logik, falls benötigt
    }

    public void Restart()
    {
        if (healthScript != null)
        {
            healthScript.ResetHealth(); // Setze den Gesundheitsstatus zurück
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WatchAdToContinue()
    {
        // Hier Unity Ads laden und Leben auffüllen
        // Gameover Screen weg machen
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

