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
        abilityManager = GetComponent<PlayerAbilityManager>();

        if (currentAbility is DoubleJumpAbility)
        {
            animator.SetTrigger("ChangeGrasshopper");
        }
        else if (currentAbility is SlashAbility)
        {
            animator.SetTrigger("ChangeMantis");
        }
        else
        {
            animator.SetTrigger("ChangeSlime");
        }
    }

    void Update()
    {
        HandleJump();
        Move();
        Flip();
        ChangeSlime();
    }
    private void ChangeSlime()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("ChangeSlime");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetTrigger("ChangeMantis");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("ChangeMantis");
        }
    }

    void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            jumpCharge = 0f;

            if (currentAbility is DoubleJumpAbility)
            {
                animator.SetBool("GrasshopperIsFall", true);
            }
            else if (currentAbility is SlashAbility)
            {
                animator.SetBool("MantisIsFall", true);
            }
            else
            {
                animator.SetBool("IsFall", true);
            }
        }

        if (Input.GetKey(KeyCode.Space) && isCharging)
        {
            if (currentAbility is DoubleJumpAbility)
            {
                animator.SetBool("GrasshopperIsFall", true);
            }
            else if (currentAbility is SlashAbility)
            {
                animator.SetBool("MantisIsFall", true);
            }
            else
            {
                animator.SetBool("IsFall", true);
            }
            
            jumpCharge += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            isCharging = false;
            if (currentAbility is DoubleJumpAbility)
            {
                animator.SetBool("GrasshopperIsFall", false);
                animator.SetBool("GrasshopperIsJumping", true);
            }
            else if (currentAbility is SlashAbility)
            {
                animator.SetBool("MantisIsFall", false);
                animator.SetBool("MantisIsJumping", true);
            }
            else
            {
                animator.SetBool("IsFall", false);
                animator.SetBool("IsJumping", true);
            }

            if (isGrounded || (doubleJump && jumpCount < 2) && currentAbility is DoubleJumpAbility) // ダブルジャンプの処理を追加
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
        AudioM.Instance.PlayJumpSound();
        rb.velocity = Vector3.zero;
        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpCharge / chargeTime);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isJumping = true;
        jumpCharge = 0f;
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
        if (currentAbility is DoubleJumpAbility)
        {
            animator.SetBool("GrasshopperIsJumping", false);
            animator.SetBool("GrasshopperIsFall", false);
        }
        else if (currentAbility is SlashAbility)
        {
            animator.SetBool("MantisIsJumping", false);
            animator.SetBool("MantisIsFall", false);
        }
        else
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFall", false);
        }
    }
    else if (collision.gameObject.CompareTag("JumpAbilities")) // アビリティ取得の処理
    {
        Ability ability = collision.gameObject.GetComponent<DoubleJumpAbility>();

        if (ability != null)
        {
            abilityManager.SetAbility(ability);
            currentAbility = ability; // currentAbilityに設定
            Destroy(collision.gameObject);
        }
        animator.SetTrigger("ChangeGrasshopper");
    }
    else if (collision.gameObject.CompareTag("SlashAbility"))
    {
        Ability ability = collision.gameObject.GetComponent<SlashAbility>();

        if (ability != null)
        {
            abilityManager.SetAbility(ability);
            currentAbility = ability; // currentAbilityに設定
            Destroy(collision.gameObject);
        }
        animator.SetTrigger("ChangeMantis");
    }
    else if (collision.gameObject.CompareTag("ProjectileAbility"))
    {
        Ability ability = collision.gameObject.GetComponent<ProjectileAbility>();

        if (ability != null)
        {
            abilityManager.SetAbility(ability);
            currentAbility = ability; // currentAbilityに設定
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
}

