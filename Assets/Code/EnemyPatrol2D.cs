using UnityEngine;
using System.Collections; // Notwendig für Coroutines

public class EnemyPatrol2D : MonoBehaviour
{
    public Transform pointA; // Der erste Punkt, zu dem der Gegner läuft
    public Transform pointB; // Der zweite Punkt, zu dem der Gegner läuft

    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    private bool facingRight = true;
    private bool isCollidingWithPlayer = false;
    private Health healthScript;

    public float speed;
    public float hearts;          // Anzahl der Herzen des Gegners
    public float max_hearts = 3;
    public float hitRate = 1f;    // Schaden, der zugefügt wird
    public float damageDelay = 1f; // Verzögerung zwischen den Schadensereignissen
    public float clickDelay = 1f; // Verzögerung zwischen den erlaubten Klicks
    public float attackRange = 3f; // Reichweite, in der der Spieler den Gegner angreifen kann

    private float lastHitTime;   // Zeitstempel des letzten Schadens
    private float lastClickTime; // Zeitstempel des letzten erlaubten Klicks
    private bool canAttack = true; // Ob der Gegner angreifen kann

    public Transform playerTransform; // Öffentliches Transform für den Spieler, im Inspector zugewiesen

    [SerializeField] private Health_Bar healthBar;  // Tippfehler korrigiert: SerilizeField -> SerializeField, FloatingHealthBar zu Health_Bar geändert

    private bool isDead = false; // Status des Gegners, ob er tot ist
    private bool isHurt = false; // Status des Gegners, ob er verletzt ist

    public float hurtAnimationDuration = 0.7f; // Dauer der Hurt-Animation
    public float deathAnimationDuration = 1.2f; // Dauer der Death-Animation

    void Start()
    {
        healthScript = FindObjectOfType<Health>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB;
        anim.SetBool("isRunning", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isHurt", false);
        anim.SetBool("isDead", false);
        healthBar = GetComponentInChildren<Health_Bar>();  // Korrigiert, um die Health_Bar-Komponente zu finden

        if (healthBar == null)
        {
            Debug.LogError("Health_Bar component not found on enemy!");
        }
    }

    void Update()
    {
        // Überprüfen, ob Punkt A und Punkt B die gleiche x-Position haben
        if (Mathf.Approximately(pointA.position.x, pointB.position.x))
        {
            // Punkt A und Punkt B haben die gleiche x-Position
            anim.SetBool("isRunning", false);
            rb.velocity = Vector2.zero; // Bewegung stoppen

            // Falls der Gegner auf den Spieler trifft oder angegriffen wird, sicherstellen, dass keine Bewegungen stattfinden
            if (isCollidingWithPlayer || isHurt || isDead || anim.GetBool("isAttacking"))
            {
                return;
            }
        }
        else
        {
            // Normaler Patrouillenmodus
            anim.SetBool("isRunning", true);

            if (isDead || isHurt || anim.GetBool("isAttacking"))
            {
                rb.velocity = Vector2.zero; // Bewegung stoppen
                return;
            }

            if (!isCollidingWithPlayer)
            {
                Vector2 direction = currentPoint.position - transform.position;
                rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed, rb.velocity.y);

                if ((currentPoint == pointB && !facingRight) || (currentPoint == pointA && facingRight))
                {
                    Flip();
                }

                if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
                {
                    currentPoint = (currentPoint == pointB) ? pointA : pointB;
                }
            }
            else if (canAttack)
            {
                if (Time.time >= lastHitTime + damageDelay)
                {
                    AttackPlayer();
                    lastHitTime = Time.time; // Zeitstempel aktualisieren
                    canAttack = false;
                    Invoke(nameof(ResetAttack), damageDelay); // Angriff nach einer Verzögerung wieder erlauben
                }
            }

            // Überprüfen, ob der Spieler innerhalb des Angriffsbereichs ist und ob die Maustaste geklickt wurde
            if (playerTransform != null && Time.time >= lastClickTime + clickDelay)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

                if (distanceToPlayer <= attackRange && Input.GetMouseButtonDown(0))
                {
                    DamageEnemy();
                    lastClickTime = Time.time; // Zeitstempel aktualisieren
                }
            }
        }

        healthBar.UpdateHealthBar(hearts, max_hearts);
    }


    void Flip()
    {
        // Speichere die aktuelle Skalierung des Canvas
        Transform canvasTransform = transform.Find("Canvas"); // Canvas transform suchen
        Vector3 canvasScale = canvasTransform.localScale;

        // Enemy umdrehen
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Setze die ursprüngliche Skalierung des Canvas zurück
        canvasTransform.localScale = canvasScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
            rb.velocity = Vector2.zero; // Bewegung sofort stoppen
            anim.SetBool("isRunning", false); // Animation stoppen

            // Direkter Schaden beim ersten Kontakt
            if (Time.time >= lastHitTime + damageDelay && canAttack)
            {
                AttackPlayer();
                lastHitTime = Time.time; // Zeitstempel aktualisieren
                canAttack = false;
                Invoke(nameof(ResetAttack), damageDelay); // Angriff nach einer Verzögerung wieder erlauben
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
            anim.SetBool("isRunning", true); // Animation wieder starten
        }
    }

    private void AttackPlayer()
    {
        if (healthScript != null)
        {
            anim.SetBool("isAttacking", true);
            healthScript.TakeDamage((int)hitRate); // Verursacht Schaden am Spieler
        }
    }

    public void DamageEnemy()
    {
        // Reduziert die Herzen des Gegners
        hearts--;
        healthBar.UpdateHealthBar(hearts, max_hearts);

        anim.SetBool("isHurt", true);
        isHurt = true; // Gegner als verletzt markieren

        // Überprüfen, ob der Gegner keine Herzen mehr hat
        if (hearts <= 0)
        {
            anim.SetBool("isDead", true);
            isDead = true; // Gegner als tot markieren

            StartCoroutine(DestroyAfterDelay(deathAnimationDuration)); // Coroutine für das Zerstören des Gegners
        }

        Debug.Log("Enemy hit! Remaining hearts: " + hearts);

        StartCoroutine(ResetHurtAnimationAfterDelay(hurtAnimationDuration)); // Coroutine für das Zurücksetzen der Hurt-Animation
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private IEnumerator ResetHurtAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("isHurt", false);
        isHurt = false; // Verletzungsstatus zurücksetzen
    }

    private void ResetAttack()
    {
        canAttack = true; // Ermöglicht dem Gegner wieder anzugreifen
        anim.SetBool("isAttacking", false); // Animation wieder stoppen
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.DrawWireSphere(pointA.position, 0.5f);
            Gizmos.DrawWireSphere(pointB.position, 0.5f);
            Gizmos.DrawLine(pointA.position, pointB.position);
        }

        // Zeichne den Angriffsbereich im Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
