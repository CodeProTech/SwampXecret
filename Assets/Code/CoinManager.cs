using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public Text coinText;

    private Vector3 originalScale; // Zum Speichern der urspr�nglichen Gr��e des Textes
    public float enlargeDuration = 0.1f; // Dauer, f�r die der Text gr��er bleibt
    public float enlargedScaleFactor = 1.01f; // Wie viel gr��er der Text wird

    private bool isUpdating = false; // Verhindert wiederholte Animationen
    private int previousCoinCount; // Zum Speichern des vordherigen M�nzenwertes

    // Start is called before the first frame update
    void Start()
    {
        originalScale = coinText.transform.localScale; // Urspr�ngliche Gr��e des Textes speichern
        previousCoinCount = coinCount; // Initialen Wert speichern
    }

    // Update is called once per frame
    void Update()
    {
        // �berpr�fe, ob sich die M�nzenanzahl ge�ndert hat
        if (coinCount != previousCoinCount && !isUpdating)
        {
            coinText.text = coinCount.ToString();
            StartCoroutine(EnlargeText());

            // Speichere den neuen Wert als vorherigen Wert
            previousCoinCount = coinCount;
        }
    }

    // Coroutine zum Vergr��ern des Textes
    IEnumerator EnlargeText()
    {
        isUpdating = true;

        // Text vergr��ern
        coinText.transform.localScale = originalScale * enlargedScaleFactor;

        // Warte die festgelegte Dauer
        yield return new WaitForSeconds(enlargeDuration);

        // Text auf die urspr�ngliche Gr��e zur�cksetzen
        coinText.transform.localScale = originalScale;

        isUpdating = false; // Erlaube n�chste Animation
    }
}
