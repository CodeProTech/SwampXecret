using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public GameObject panel; // Ziehe dein Panel hier rein
    public GameObject player; // Hier den Spieler im Inspector zuweisen
    public Vector2 teleportPosition = new Vector2(1000f, 1000f); // Zielposition im 2D-Raum

    public CameraShake cameraShake; // Referenz zum CameraShake-Skript

    public bool isGameOver = false;

    private Rigidbody2D rb; // Ändere zu Rigidbody2D, da du in 2D arbeitest
    private Animator animator;

    private string currentstate;

    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>(); // Hole das Rigidbody2D-Component vom Spieler
        animator = transform.Find("PlayerAnim").GetComponentInChildren<Animator>();
        panel.SetActive(false);
    }

    void Update()
    {
        if (health <= 0)
        {
            ShowPanel();
            player.transform.position = teleportPosition; // Setzt nur X und Y
            rb.gravityScale = 0; // Gravitation ausschalten
        }

        // Herz-UI aktualisieren
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            hearts[i].enabled = i < numOfHearts;
        }
    }

    // Zeigt das Panel an
    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    // Schadensfunktion
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            gameOver();
        }

        // Kamera wackeln lassen, wenn der Spieler Schaden nimmt
        if (cameraShake != null)
        {
            cameraShake.TriggerShake();
            ChangeAnimationState("player_hit");
        }
        else
        {
            Debug.LogError("CameraShake Referenz ist null.");
        }
    }

    // Spiel anhalten, wenn der Spieler tot ist
    void gameOver()
    {
        isGameOver = true;
        ChangeAnimationState("player_dead");

    }

    // Setzt die Gesundheit zurück
    public void ResetHealth()
    {
        health = numOfHearts;
        Time.timeScale = 1f; // Setze die Zeit-Skalierung zurück
    }

    IEnumerator PauseMachen()
    {
        Debug.Log("Pause startet...");
        yield return new WaitForSeconds(1.5f); // 1 Sekunde warten
        Debug.Log("Pause vorbei!");
    }


    void ChangeAnimationState(string newState)
    {
        if (currentstate != newState)
        {
            currentstate = newState;
            animator.CrossFade(newState, 0.2f);
        }

        animator.Play(newState);

        currentstate = newState;

    }
}

