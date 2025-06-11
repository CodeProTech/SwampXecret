using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    public GameObject movingObject; // Das bewegliche Objekt
    public GameObject rectangleObject; // Das Rechteck
    public GameObject panel; // Das Panel

    private void Start()
    {
        // Stelle sicher, dass das Panel zu Beginn versteckt ist
        panel.SetActive(false);
    }

    private void Update()
    {
        // Überprüfe, ob das bewegliche Objekt das Rechteck berührt
        if (IsColliding(movingObject, rectangleObject))
        {
            // Zeige das Panel an
            panel.SetActive(true);
        }
    }

    private bool IsColliding(GameObject obj1, GameObject obj2)
    {
        // Hol die Kollisionsboxen der beiden Objekte
        Collider2D collider1 = obj1.GetComponent<Collider2D>();
        Collider2D collider2 = obj2.GetComponent<Collider2D>();

        // Überprüfe, ob die Kollisionsboxen existieren und ob sie sich überlappen
        return collider1 != null && collider2 != null && collider1.bounds.Intersects(collider2.bounds);
    }
}
