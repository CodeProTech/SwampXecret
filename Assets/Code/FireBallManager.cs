using UnityEngine;
using UnityEngine.UI;

public class FireballManager : MonoBehaviour
{
    public int initialFireballCount = 20;  // Anfangszahl der Fireballs
    public Text fireballCountText;         // Referenz zum UI-Textobjekt

    private int currentFireballCount;

    void Start()
    {
        // Initialisiere die Anzahl der Fireballs
        currentFireballCount = initialFireballCount;
        UpdateFireballCountText();
    }

    public bool TryShootFireball()
    {
        if (currentFireballCount > 0)
        {
            currentFireballCount--;  // Reduziere die aktuelle Anzahl der Fireballs
            UpdateFireballCountText();  // Aktualisiere das UI
            return true;
        }
        return false;
    }

    private void UpdateFireballCountText()
    {
        fireballCountText.text = currentFireballCount.ToString();
    }
}
