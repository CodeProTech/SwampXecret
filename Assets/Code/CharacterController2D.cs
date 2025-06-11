using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;  // Füge diesen Namespace hinzu
using System.Collections.Generic; // Füge diesen Namespace ebenfalls hinzu


public class CharacterController2D : MonoBehaviour
{
    [Header("SFX")]
    public AudioSource run_SFX;
    public AudioSource attack_SFX;
    public AudioSource hurt_SFX;
    public AudioSource die_SFX;
    public AudioSource jump_SFX;
    public AudioSource fall_SFX;
    public AudioSource dash_SFX;

    [Header("Buttons")]
    [SerializeField] private Button RunLeftButton;
    [SerializeField] private Button RunRightButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button dashButton;
    [SerializeField] private Button attackButton;

    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, 1)][SerializeField] private float m_CrouchSpeed = .36f;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;
    [SerializeField] private Collider2D m_CrouchDisableCollider;

    [Header("Arrow Settings")]
    [SerializeField] private GameObject arrowPrefab; // Pfeil-Prefab
    [SerializeField] private Transform arrowSpawnPoint; // Der Punkt, von dem aus der Pfeil abgefeuert wird
    [SerializeField] private Sprite arrowSprite; // Die Textur für den Pfeil

    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f; // Dauer des Cooldowns nach dem Dash

    [Header("Jump Settings")]
    [SerializeField] private int maxJumps = 2;

    [Header("Wall Jump Settings")]
    [SerializeField] private Transform m_WallCheck;
    [SerializeField] private float wallCheckRadius = 0.2f;
    [SerializeField] private float wallJumpForce = 400f;
    [SerializeField] private Vector2 wallJumpDirection = new Vector2(1, 1);
    [SerializeField] private float wallSlideSpeed = 2f;

    private bool isDashing = false;
    private float dashTimeLeft;
    private float originalGravityScale;
    private bool canDash = true;

    private const float k_GroundedRadius = .2f;
    private bool m_Grounded;
    private const float k_CeilingRadius = .2f;
    private const float k_WallRadius = .2f;
    private bool m_TouchingWall;
    private Rigidbody2D m_Rigidbody2D;
    public bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;

    private int jumpCount = 0;

    private string currentstate;
    private bool isAttacking = false;
    private float lastXPosition;

    [Header("Events")]
    public UnityEvent OnLandEvent;
    public BoolEvent OnCrouchEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private bool m_wasCrouching = false;
    public CoinManager cm;
    public Health health;
    private Animator animator;
    public GameObject GAMEOVERpanel;

    // Track if the movement buttons are being held down
    private bool isRunningLeft = false;
    private bool isRunningRight = false;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        originalGravityScale = m_Rigidbody2D.gravityScale;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void Start()
    {
        // Button listeners
        AddButtonListener(RunLeftButton, () => isRunningLeft = true, () => isRunningLeft = false);
        AddButtonListener(RunRightButton, () => isRunningRight = true, () => isRunningRight = false);

        jumpButton.onClick.AddListener(() => Jump());
        dashButton.onClick.AddListener(() => Dash());
        attackButton.onClick.AddListener(() => Attack());

        cm = FindObjectOfType<CoinManager>();
        animator = transform.Find("PlayerAnim").GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        lastXPosition = transform.position.x;
        // isAttacking = false;
        // Handle continuous movement
        if (isAttacking)
        {
            ChangeAnimationState("player_attack_idel_1");
        }
        else if (transform.position.y < -9)
        {
            GAMEOVERpanel.SetActive(true); // Panel anzeigen, wenn das Objekt unter -9 fällt
        }
        else if (!Input.anyKey && !isDashing && m_Grounded && !Input.GetMouseButtonDown(0) && !isAttacking) // IMPORTANT if button mous 0 is clicked no plasyer_idel animation fin another solution
        {
            ChangeAnimationState("player_idel");
        }
        else if (!m_Grounded && !isDashing && !isAttacking)
        {
            ChangeAnimationState("player_jump");
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            {
                float move = Input.GetAxis("Horizontal"); // Get movement input
                StartDash(move);
            }
            if (Input.GetMouseButtonDown(0))
            {
                AttackAnimation();
            }
        }
        else if (isRunningLeft || Input.GetKey(KeyCode.A) && !isAttacking)
        {
            RunLeft();

            if (!isDashing && !Input.GetMouseButtonDown(0) && !isAttacking) ChangeAnimationState("player_run"); // animation run

            if (isDashing && !Input.GetMouseButtonDown(0) && !isAttacking) ChangeAnimationState("player_dash");

            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !Input.GetMouseButtonDown(0) && !isAttacking)
            {
                float move = Input.GetAxis("Horizontal"); // Get movement input
                StartDash(move);
            }
            if (Input.GetMouseButtonDown(0))
            {
                ChangeAnimationState("player_attack_run_2");
                isAttacking = true;
                Debug.Log("Bonjorno");
            }

        }

        else if (isRunningRight || Input.GetKey(KeyCode.D) && !isAttacking)
        {
            RunRight();
            if (!isDashing) ChangeAnimationState("player_run"); // animation run

            if (isDashing) ChangeAnimationState("player_dash");

            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            {
                float move = Input.GetAxis("Horizontal"); // Get movement input
                StartDash(move);
            }
            if (Input.GetMouseButtonDown(0))
            {
                ChangeAnimationState("player_attack_run_1");
                isAttacking = true;
            }
        }

        else if (isDashing && !isAttacking) ChangeAnimationState("player_dash");

        else if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !isAttacking)
        {
            float move = Input.GetAxis("Horizontal"); // Get movement input
            StartDash(move);
        }

        else if (Input.GetKeyDown(KeyCode.Space) && !m_Grounded && !isAttacking) ChangeAnimationState("player_jump");

        else if (Input.GetMouseButtonDown(0))
        {
            ChangeAnimationState("player_attack_idel_1");
            isAttacking = true;
        }
        else if (health.isGameOver) ChangeAnimationState("player_dead");

        isAttacking = false;
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // Check if the character is grounded
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < groundColliders.Length; i++)
        {
            if (groundColliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                    jumpCount = 0;
                }
            }
        }

        // Check if the character is touching a wall
        m_TouchingWall = Physics2D.OverlapCircle(m_WallCheck.position, wallCheckRadius, m_WhatIsGround);

        if (m_TouchingWall && !m_Grounded)
        {
            // Wall Sliding Logic
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, -wallSlideSpeed, float.MaxValue));
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.fixedDeltaTime;
            if (dashTimeLeft <= 0)
            {
                StopDash();
            }
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (isDashing) return;

        if (!crouch)
        {
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        if (true)
        {
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                move *= m_CrouchSpeed;

                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }

        if (jump)
        {
            if (m_Grounded || jumpCount < maxJumps)
            {
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                jumpCount++;
            }
            else if (m_TouchingWall && !m_Grounded)
            {
                // Wall Jump
                Vector2 jumpDirection = new Vector2(wallJumpDirection.x * (m_FacingRight ? -1 : 1), wallJumpDirection.y);
                m_Rigidbody2D.AddForce(jumpDirection * wallJumpForce);
                jumpCount = 1;  // Set jumpCount to 1 so player can jump once more after wall jump
                Flip();
            }
        }
    }

    private void RunLeft()
    {
        float move = -1f;  // Bewegung nach links

        if (isDashing) return;

        HandleCrouching(false);

        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (!m_FacingRight)
        {
            // Flip();
        }
    }

    private void RunRight()
    {
        float move = 1f;  // Bewegung nach rechts

        if (isDashing) return;

        HandleCrouching(false);

        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (m_FacingRight)
        {
            // Flip();
        }
    }

    private void Jump()
    {
        if (m_Grounded || jumpCount < maxJumps)
        {

            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            jumpCount++;
            jump_SFX.Play();
            ChangeAnimationState("player_jump");
        }
        else if (m_TouchingWall && !m_Grounded)
        {
            // Wall Jump
            Vector2 jumpDirection = new Vector2(wallJumpDirection.x * (m_FacingRight ? -1 : 1), wallJumpDirection.y);
            m_Rigidbody2D.AddForce(jumpDirection * wallJumpForce);
            jumpCount = 1;  // Set jumpCount to 1 so player can jump once more after wall jump
            Flip();
            jump_SFX.Play();
            ChangeAnimationState("player_jump");
        }
    }

    private void Dash()
    {

        if (canDash && !isDashing)
        {
            Debug.Log("Starting Dash");
            StartCoroutine(DashRoutine());
            ChangeAnimationState("player_dash");
        }
        else
        {
            Debug.Log("Cannot dash: " + (canDash ? "isDashing is true" : "canDash is false"));
        }
    }

    private IEnumerator DashRoutine()
    {
        canDash = false; // Dash ist nicht mehr möglich
        isDashing = true;
        dash_SFX.Play();

        Debug.Log("Dash started");

        float moveDirection = m_FacingRight ? 1f : -1f;
        m_Rigidbody2D.velocity = new Vector2(moveDirection * dashForce, 0f);
        m_Rigidbody2D.gravityScale = 0f;

        yield return new WaitForSeconds(dashTime); // Warte für die Dauer des Dashs

        Debug.Log("Dash time over, stopping dash");

        StopDash(); // Dash beenden

        yield return new WaitForSeconds(dashCooldown); // Warte für die Dauer des Cooldowns

        Debug.Log("Cooldown over, canDash is true");

        canDash = true; // Dash ist wieder möglich
    }

    private IEnumerator AttackAnimation()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }
    private void Attack()
    {
        Debug.Log("Attack");
        attack_SFX.Play();
        ChangeAnimationState("player_attack_idel_1");
    }

    private void HandleCrouching(bool crouch)
    {
        if (!crouch)
        {
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        if (crouch)
        {
            if (!m_wasCrouching)
            {
                m_wasCrouching = true;
                OnCrouchEvent.Invoke(true);
            }

            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = false;
        }
        else
        {
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = true;

            if (m_wasCrouching)
            {
                m_wasCrouching = false;
                OnCrouchEvent.Invoke(false);
            }
        }
    }

    private void StartDash(float move)
    {
        isDashing = true;
        dashTimeLeft = dashTime;

        m_Rigidbody2D.gravityScale = 0f;

        float dashDirection = move != 0 ? Mathf.Sign(move) : (m_FacingRight ? 1 : -1);
        m_Rigidbody2D.velocity = new Vector2(dashDirection * dashForce, 0f);
    }

    private void StopDash()
    {
        isDashing = false;
        m_Rigidbody2D.gravityScale = originalGravityScale;
        m_Rigidbody2D.velocity = Vector2.zero;
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Coin detected!");

            Coin coin = other.gameObject.GetComponent<Coin>();
            if (coin != null)
            {
                if (!coin.isCollected)
                {
                    Debug.Log("Coin is not collected yet.");
                    coin.isCollected = true; // Markiere die spezifische Münze als eingesammelt
                    cm.coinCount++; // Erhöhe die Münzanzahl
                    Destroy(other.gameObject); // Zerstöre die Münze
                }
                else
                {
                    Debug.Log("Coin already collected.");
                }
            }
            else
            {
                Debug.Log("No Coin script found on coin.");
            }
        }
    }

    // AddButtonListener: Helper method to handle PointerDown and PointerUp for continuous movement
    private void AddButtonListener(Button button, UnityAction onPointerDown, UnityAction onPointerUp)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((data) => { onPointerDown(); });
        trigger.triggers.Add(entryDown);

        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((data) => { onPointerUp(); });
        trigger.triggers.Add(entryUp);
    }

    void ChangeAnimationState(string newState)
    {
        if (currentstate != newState)
        {
            currentstate = newState;
            animator.CrossFade(newState, 0.2f);
        }

        animator.Play(newState);

        currentstate = newState;

    }
}