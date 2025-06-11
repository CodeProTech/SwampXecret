using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PortalTransition : MonoBehaviour
{
    public float shrinkSpeed = 2f; // Geschwindigkeit, mit der der Spieler schrumpft
    public float moveSpeed = 5f;   // Geschwindigkeit, mit der der Spieler zur Mitte gezogen wird
    public GameObject panel; // Ziehe dein Panel hier rein

    private bool isTeleporting = false;
    private Transform player;
    private Vector3 portalCenter;

    void Start()
    {
        // Berechnung des genauen Zentrums basierend auf Collider des Portals
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            portalCenter = collider.bounds.center;
        }
        else
        {
            portalCenter = transform.position; // Falls kein Collider existiert, Standardposition nehmen
        }

        // Sicherstellen, dass das Panel deaktiviert ist
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            Debug.Log("Kollidiert mit dem Portal");
            isTeleporting = true;
            player = other.transform;
            StartCoroutine(TeleportPlayer());
        }
    }

    IEnumerator TeleportPlayer()
    {
        while (player.localScale.x > 0.05f) // Solange der Spieler noch sichtbar ist
        {
            // Spieler exakt zur Mitte des Portals bewegen
            player.position = Vector3.Lerp(player.position, portalCenter, moveSpeed * Time.deltaTime);

            // Spieler schrumpfen lassen
            player.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(1f); // 1 Sekunde warten

        // Panel aktivieren, anstatt die Szene zu wechseln
        if (panel != null)
        {
            panel.SetActive(true);
            yield return new WaitForSeconds(1f); // 1 Sekunde warten
            SceneManager.LoadScene(7);

        }
    }
}

