using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGameWithMenu : MonoBehaviour
{
    public Button pauseButton;
    public Button mainMenuButton;
    public Button restartButton;
    public Button continueButton;
    public GameObject menuPanel;
    private bool isPaused = false;

    void Start()
    {
        // Prüfe, ob alle Buttons und das Menü-Panel zugewiesen sind
        if (!pauseButton || !mainMenuButton || !restartButton || !continueButton || !menuPanel)
        {
            Debug.LogError("Einer oder mehrere Buttons bzw. das Menü-Panel sind nicht zugewiesen!");
            return;
        }

        // Listener hinzufügen
        pauseButton.onClick.AddListener(TogglePause);
        mainMenuButton.onClick.AddListener(MainMenu);
        restartButton.onClick.AddListener(Restart);
        continueButton.onClick.AddListener(Continue);

        // Menü-Panel standardmäßig deaktivieren
        menuPanel.SetActive(false);
    }

    void TogglePause()
    {
        // Schalte den Pausenstatus um
        isPaused = !isPaused;

        // Menü-Panel direkt sichtbar machen oder verstecken
        menuPanel.SetActive(isPaused);

        // Pausiere oder setze das Spiel fort
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Restart()
    {
        // Setze die Spielzeit zurück und lade die aktuelle Szene neu
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Continue()
    {
        // Setze das Spiel fort, indem du das Menü-Panel deaktivierst und die Zeit wieder normal laufen lässt
        if (isPaused)
        {
            menuPanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void MainMenu()
    {
        // Setze die Spielzeit zurück und lade das Hauptmenü
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Stelle sicher, dass die Szene mit Index 0 das Hauptmenü ist
    }
}

