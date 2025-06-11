using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;  // Geschwindigkeit des Fireballs
    public int damage = 1;     // Schaden, den der Fireball verursacht

    private Rigidbody2D rb;
    private CharacterController2D cc2d;
    private Vector2 direction;
    private FireballManager fireballManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc2d = FindObjectOfType<CharacterController2D>();
        fireballManager = FindObjectOfType<FireballManager>();

        if (rb != null && cc2d != null && fireballManager != null)
        {
            if (cc2d.m_FacingRight == true)
            {
                direction = Vector2.right; // Feuerball nach rechts schieﬂen
            }
            else
            {
                direction = Vector2.left;  // Feuerball nach links schieﬂen
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            rb.velocity = direction * speed;
            rb.gravityScale = 0;

        }
        else
        {
            Debug.LogError("Rigidbody2D, CharacterController2D oder FireballManager fehlt.");
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyPatrol2D enemy = hitInfo.GetComponent<EnemyPatrol2D>();

        if (enemy != null)
        {
            enemy.DamageEnemy();
            Destroy(gameObject);
        }
    }
}

