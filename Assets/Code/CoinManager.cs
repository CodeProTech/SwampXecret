using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public Text coinText;

    private Vector3 originalScale; // Zum Speichern der ursprünglichen Größe des Textes
    public float enlargeDuration = 0.1f; // Dauer, für die der Text größer bleibt
    public float enlargedScaleFactor = 1.01f; // Wie viel größer der Text wird

    private bool isUpdating = false; // Verhindert wiederholte Animationen
    private int previousCoinCount; // Zum Speichern des vordherigen Münzenwertes

    // Start is called before the first frame update
    void Start()
    {
        originalScale = coinText.transform.localScale; // Ursprüngliche Größe des Textes speichern
        previousCoinCount = coinCount; // Initialen Wert speichern
    }

    // Update is called once per frame
    void Update()
    {
        // Überprüfe, ob sich die Münzenanzahl geändert hat
        if (coinCount != previousCoinCount && !isUpdating)
        {
            coinText.text = coinCount.ToString();
            StartCoroutine(EnlargeText());

            // Speichere den neuen Wert als vorherigen Wert
            previousCoinCount = coinCount;
        }
    }

    // Coroutine zum Vergrößern des Textes
    IEnumerator EnlargeText()
    {
        isUpdating = true;

        // Text vergrößern
        coinText.transform.localScale = originalScale * enlargedScaleFactor;

        // Warte die festgelegte Dauer
        yield return new WaitForSeconds(enlargeDuration);

        // Text auf die ursprüngliche Größe zurücksetzen
        coinText.transform.localScale = originalScale;

        isUpdating = false; // Erlaube nächste Animation
    }
}
