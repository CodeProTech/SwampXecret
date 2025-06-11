using System.Collections;
using UnityEngine;

public class CoinMovement2D : MonoBehaviour
{
    public float movementSpeed = 0.5f;  // Geschwindigkeit der Bewegung
    public float movementAmount = 30f;  // Bewegung in Pixel (30 Pixel entsprechen ca. 0,3 Units in Unity)

    private Vector3 startPosition;
    private float offset;

    void Start()
    {
        // Initiale Position speichern
        startPosition = transform.position;

        // Zufälliger Phasenversatz für jede Münze, damit sie nicht synchron sind
        offset = Random.Range(0f, 2f * Mathf.PI);

        // Starte die Bewegung als Coroutine
        StartCoroutine(MoveCoin());
    }

    IEnumerator MoveCoin()
    {
        while (true)
        {
            // Berechne die neue Position basierend auf Sinus-Wellenbewegung
            float newY = startPosition.y + Mathf.Sin(Time.time * movementSpeed + offset) * (movementAmount / 100f);
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);

            yield return null;  // Warte bis zum nächsten Frame
        }
    }
}

