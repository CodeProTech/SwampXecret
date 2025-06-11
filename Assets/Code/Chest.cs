using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;
    private bool isPlayerInRange = false;
    private bool isChestOpen = false; // Startzustand der Truhe ist geschlossen
    private CoinManager coin;

    void Start()
    {
        // Holen der Animator-Komponente vom Chest-GameObject
        animator = GetComponent<Animator>();
        coin = FindObjectOfType<CoinManager>();
    }

    void Update()
    {
        // Überprüft, ob der Spieler im Bereich ist, geklickt hat und die Truhe noch geschlossen ist
        if (isPlayerInRange && Input.GetMouseButtonDown(0) && !isChestOpen)
        {
            OpenChest(); // Truhe öffnen
        }
    }

    private void OpenChest()
    {
        isChestOpen = true; // Truhe ist jetzt geöffnet
        animator.SetBool("isOpen", true); // Animator-Parameter setzen

        // Zufallswert generieren
        float randomValue = Random.Range(0f, 100f);

        // Münzverteilung nach Wahrscheinlichkeiten
        if (randomValue <= 50f)  // 50% Wahrscheinlichkeit
        {
            coin.coinCount += 5;
        }
        else if (randomValue <= 75f)  // 25% Wahrscheinlichkeit
        {
            coin.coinCount += Random.Range(5, 8);  // 5-7 Münzen
        }
        else if (randomValue <= 87.5f)  // 12.5% Wahrscheinlichkeit
        {
            coin.coinCount += Random.Range(8, 11);  // 8-10 Münzen
        }
        else if (randomValue <= 90.5f)  // 3% Wahrscheinlichkeit
        {
            coin.coinCount += Random.Range(10, 16);  // 10-15 Münzen
        }
        else if (randomValue <= 91.5f)  // 1% Wahrscheinlichkeit
        {
            coin.coinCount += 20;  // 20 Münzen
        }
        else if (randomValue <= 91.55f)  // 0.05% Wahrscheinlichkeit
        {
            coin.coinCount += 50;  // 50 Münzen
        }
    }

    // Wird aufgerufen, wenn der Spieler in den Trigger-Bereich eintritt
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; // Spieler ist im Bereich
        }
    }

    // Wird aufgerufen, wenn der Spieler den Trigger-Bereich verlässt
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; // Spieler ist nicht mehr im Bereich
        }
    }
}
