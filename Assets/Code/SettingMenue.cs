using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu; // Das UI-Menü für die Einstellungen
    public Button closeButton; // Der Button, um das Menü zu schließen

    private bool isPaused = false;

    void Start()
    {
        // Das Menü und den Button initialisieren
        settingsMenu.SetActive(false);
        closeButton.onClick.AddListener(CloseSettingsMenu);
    }

    void Update()
    {
        // Das Menü öffnen, wenn die Escape-Taste gedrückt wird
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
