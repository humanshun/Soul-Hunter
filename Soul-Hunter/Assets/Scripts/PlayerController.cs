using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;     // 移動速度
    public float maxJumpForce = 10f; // 最大ジャンプ力
    public float chargeTime = 1f;    // 最大ジャンプ力に達するまでの時間
    public float minJumpForce = 2f;  // 最小ジャンプ力
    public string groundTag = "Ground"; // 地面のタグ

    public Transform groundCheck;     // 地面判定用の子オブジェクト
    public float groundCheckRadius = 0.2f; // 地面判定用の半径

    private bool isCharging = false;
    private float jumpCharge = 0f;
    private Rigidbody2D rb;
    private bool isGrounded = false;  // プレイヤーが地面にいるかどうかを判定するためのフラグ
    private bool isJumping = false;   // プレイヤーがジャンプしているかどうかを判定するフラグ
    private Animator animator;
    private AnimationClip JumpAnimation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleJump();
        Move();
        Flip();
    }

    void Move()
    {
        if (isJumping) // ジャンプ中のみ移動
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    void HandleJump()
    {
        // 地面にいるかどうかを判定
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask(groundTag));

        // スペースキーが押され始めた
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isCharging = true;
            jumpCharge = 0f;
        }

        // スペースキーが押されている間
        if (Input.GetKey(KeyCode.Space) && isCharging)
        {
            jumpCharge += Time.deltaTime;
        }

        // スペースキーが離された
        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            isCharging = false;
            // JumpAnimation.SetBool(true);
            Jump();
        }
    }

    void Jump()
    {
        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpCharge / chargeTime);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isJumping = true; // ジャンプ中フラグを設定
    }

    void Flip()
    {
        // プレイヤーの移動方向に応じて向きを反転
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
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = false;
        }
    }

    // Gizmosで地面判定の半径を視覚化する
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
