using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject settingsMenu; // Das UI-Men� f�r die Einstellungen
    public GameObject overlay; // Das unsichtbare Overlay
    public Button openSettingsButton; // Der Button, um das Men� zu �ffnen
    public Button closeSettingsButton; // Der Button, um das Men� zu schlie�en
    public List<GameObject> buttonsToHide; // Die Buttons, die unsichtbar gemacht werden sollen

    void Start()
    {
        // Das Men� und das Overlay initialisieren
        settingsMenu.SetActive(false);
        overlay.SetActive(false);

        // Event-Listener f�r die Buttons hinzuf�gen
        openSettingsButton.onClick.AddListener(OpenSettingsMenu);
        closeSettingsButton.onClick.AddListener(CloseSettingsMenu);
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        overlay.SetActive(true); // Das Overlay aktivieren
        Time.timeScale = 0f; // Das Spiel pausieren

        // Mache die Buttons unsichtbar
        foreach (GameObject button in buttonsToHide)
        {
            button.SetActive(false);
        }
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        overlay.SetActive(false); // Das Overlay deaktivieren
        Time.timeScale = 1f; // Das Spiel fortsetzen

        // Mache die Buttons wieder sichtbar
        foreach (GameObject button in buttonsToHide)
        {
            button.SetActive(true);
        }
    }
}
