using System.Collections;
using UnityEngine;

public class AdvancedEnemyController : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 1.5f; // Angriffsreichweite
    public float pushBackDistance = 100f; // Abstand in Pixeln für das Zurückstoßen
    public int maxHealth = 3;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public Transform wallCheck;

    public Transform player;

    private int currentHealth;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool isHittingWall;
    private float groundCheckRadius = 0.2f;
    private float wallCheckRadius = 0.1f;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Überprüfe, ob der Gegner auf dem Boden steht und eine Wand berührt
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isHittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, groundLayer);

        // Wenn der Gegner eine Wand erreicht oder am Rand ist, dreht er um
        if (isHittingWall || !isGrounded)
        {
            Flip();
        }

        // Bewegung und Animation setzen, wenn nicht angegriffen wird
        if (!isAttacking)
        {
            rb.velocity = new Vector2(speed * (isFacingRight ? 1 : -1), rb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }

        // Überprüfen, ob der Spieler in der Nähe ist, um anzugreifen
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero; // Gegner stoppt

        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f); // Warte 1 Sekunde

        // Logik zum Angriff auf den Spieler (z.B. Herz abziehen)
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            // Füge Schaden zu
            player.GetComponent<Health>().TakeDamage(1);
        }

        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        // Gegner zurückstoßen
        float pushDirection = isFacingRight ? -1 : 1;
        rb.AddForce(new Vector2(pushDirection * pushBackDistance, 0));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            TakeDamage(1);
        }
    }

    void Die()
    {
        // Sterbeanimation abspielen und Gegner zerstören
        animator.SetTrigger("Die");
        Destroy(gameObject, 0.5f); // Kurze Verzögerung vor der Zerstörung, damit die Animation abgespielt wird
    }

    void OnDrawGizmosSelected()
    {
        // Zeigt die Reichweite des Angriffs im Editor an
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
