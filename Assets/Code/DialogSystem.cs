using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogSystem : MonoBehaviour
{
    public Canvas dialogCanvas; // Canvas f�r den Dialog
    public Text nameText;       // Text-Element f�r den Namen
    public Text dialogueText;   // Text-Element f�r den Dialog
    public string playerName;   // Name des Spielers oder Sprechers
    public string[] sentences;  // Array von S�tzen, die angezeigt werden sollen

    private int sentenceIndex = 0; // Aktueller Index des Satzes
    private bool isTyping = false;  // Flag, um zu �berpr�fen, ob der Text gerade getippt wird

    private void Start()
    {
        dialogCanvas.gameObject.SetActive(false); // Canvas zu Beginn deaktivieren
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Wenn auf den Bildschirm geklickt wird
        {
            if (!isTyping) // Sicherstellen, dass nicht w�hrend des Tippens weiter geklickt wird
            {
                DisplayNextSentence();
            }
        }
    }

    public void TriggerDialog()
    {
        if (!dialogCanvas.gameObject.activeSelf) // Nur aktivieren, wenn Canvas nicht bereits aktiv ist
        {
            dialogCanvas.gameObject.SetActive(true);
            sentenceIndex = 0;
            DisplaySentence();
        }
    }

    private void DisplaySentence()
    {
        if (sentenceIndex < sentences.Length)
        {
            nameText.text = playerName; // Setze den Namen
            StartCoroutine(TypeSentence(sentences[sentenceIndex])); // Starte die Coroutine f�r den Text
        }
        else
        {
            EndDialog(); // Wenn alle S�tze angezeigt wurden, beende den Dialog
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true; // Typing-Flag setzen
        dialogueText.text = ""; // Textfeld zur�cksetzen
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // F�ge jeden Buchstaben hinzu
            yield return new WaitForSeconds(0.05f); // Geschwindigkeit des Tippens anpassen
        }
        isTyping = false; // Typing-Flag zur�cksetzen
    }

    private void DisplayNextSentence()
    {
        sentenceIndex++;
        if (sentenceIndex < sentences.Length)
        {
            DisplaySentence(); // Zeige den n�chsten Satz an
        }
        else
        {
            EndDialog(); // Wenn keine S�tze mehr vorhanden sind, beende den Dialog
        }
    }

    private void EndDialog()
    {
        dialogCanvas.gameObject.SetActive(false); // Deaktiviere das Canvas
    }
}
