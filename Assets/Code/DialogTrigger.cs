using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    public DialogSystem dialogSystem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Überprüfen, ob der Spieler das Objekt berührt
        {
            dialogSystem.TriggerDialog();
        }
    }
}
