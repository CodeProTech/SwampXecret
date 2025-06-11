using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu; // Das UI-Men� f�r die Einstellungen
    public Button closeButton; // Der Button, um das Men� zu schlie�en

    private bool isPaused = false;

    void Start()
    {
        // Das Men� und den Button initialisieren
        settingsMenu.SetActive(false);
        closeButton.onClick.AddListener(CloseSettingsMenu);
    }

    void Update()
    {
        // Das Men� �ffnen, wenn die Escape-Taste gedr�ckt wird
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                CloseSettingsMenu();
            }
            else
            {
                OpenSettingsMenu();
            }
        }
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        Time.timeScale = 0f; // Das Spiel pausieren
        isPaused = true;
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        Time.timeScale = 1f; // Das Spiel fortsetzen
        isPaused = false;
    }
}
