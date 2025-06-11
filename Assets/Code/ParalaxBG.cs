using UnityEngine;

public class ParalaxBG : MonoBehaviour
{
    public Transform player;  // Verweis auf den Spieler
    public Vector2 parallaxEffectMultiplier;  // Geschwindigkeit des Parallax-Effekts pro Ebene

    private Vector3 lastPlayerPosition;
    private float textureUnitSizeX;

    void Start()
    {
        lastPlayerPosition = player.position;

        // Größe der Textur auf der X-Achse herausfinden
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void Update()
    {
        // Unterschiedliche Geschwindigkeiten für Parallax anwenden
        Vector3 deltaMovement = player.position - lastPlayerPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y, 0);
        lastPlayerPosition = player.position;

        // Optional: Hintergrund wiederholen lassen
        if (Mathf.Abs(player.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetX = (player.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(player.position.x + offsetX, transform.position.y);
        }
    }
}
