using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    public DialogSystem dialogSystem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �berpr�fen, ob der Spieler das Objekt ber�hrt
        {
            dialogSystem.TriggerDialog();
        }
    }
}
