using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // パラメータやコンポーネントの宣言
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float maxJumpForce = 0f;
    [SerializeField] private float chargeTime = 0f;
    [SerializeField] private float minJumpForce = 0f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0f;
    
    private bool isCharging = false;
    private float jumpCharge = 0f;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isJumping = false;
    private Animator animator;
    public bool doubleJump = false;
    private int jumpCount = 0;
    private PlayerAbilityManager abilityManager; // アビリティマネージャーを追加
    private Ability currentAbility;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        abilityManager = GetComponent<PlayerAbilityManager>(); // アビリティマネージャーを取得
    }

    void Update()
    {
        HandleJump();
        Move();
        Flip();
    }

    void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            jumpCharge = 0f;
            animator.SetBool("IsFall", true);
        }

        if (Input.GetKey(KeyCode.Space) && isCharging)
        {
            animator.SetBool("IsFall", true);
            jumpCharge += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            isCharging = false;
            animator.SetBool("IsFall", false);
            animator.SetBool("IsJumping", true);

            if (isGrounded || (doubleJump && jumpCount < 2)) // ダブルジャンプの処理を追加
            {
                Jump();
                jumpCount++;
            }
        }
    }

    void Move()
    {
        if (isJumping)
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    void Jump()
    {
        rb.velocity = Vector3.zero;
        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpCharge / chargeTime);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isJumping = true;
    }

    void Flip()
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0 && transform.localScale.x < 0 || moveInput < 0 && transform.localScale.x > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Thorn"))
        {
            isGrounded = true;
            isJumping = false;
            jumpCount = 0;
            animator.SetBool("IsFall", true);
            StartCoroutine(StopAnimationCoroutine());
        }
        else if (collision.gameObject.CompareTag("JumpAbilities")) // アビリティ取得の処理
        {
            Ability ability = collision.gameObject.GetComponent<DoubleJumpAbility>();

            if (ability != null)
            {
                abilityManager.SetAbility(ability);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("ProjectileAbility"))
        {
            Ability ability = collision.gameObject.GetComponent<ProjectileAbility>();

            if (ability != null)
            {
                abilityManager.SetAbility(ability);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("SlashAbility"))
        {
            Ability ability = collision.gameObject.GetComponent<SlashAbility>();

            if (ability != null)
            {
                abilityManager.SetAbility(ability);
                Destroy(collision.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            rb.velocity = new Vector2(rb.velocity.x, maxJumpForce);
            jumpCount = 1;
        }
    }

    IEnumerator StopAnimationCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFall", false);
    }
}

