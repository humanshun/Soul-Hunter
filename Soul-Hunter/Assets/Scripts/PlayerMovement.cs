using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0f;          //移動速度
    public float maxJumpForce = 0f;       //最大ジャンプ力
    public float chargeTime = 0f;         //最大ジャンプ力に達するまでの時間
    public float minJumpForce = 0f;       //最小ジャンプ力
    public string groundTag = "Ground";   //地面のタグ
    public Transform groundCheck;         //地面判定用の子オブジェクト
    public float groundCheckRadius = 0f;  //地面判定用の半径

    private bool isCharging = false;      //チャージしているかどうか
    private float jumpCharge = 0f;        //チャージ時間
    private Rigidbody2D rb;
    private bool isGrounded = false;      //プレイヤーが地面にいるかどうか
    private bool isJumping = false;       //プレイヤーがジャンプしているかどうか
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleJump();
        Move();
        Flip();
    }

    //　動き
    void Move()
    {
        if (isJumping) //ジャンプ中のみ移動
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    //溜めジャンプ
    void HandleJump()
    {
        //地面にいるかどうかを判定
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask(groundTag));

        //スペースキーが押され始めた
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            jumpCharge = 0f;
            animator.SetBool("IsFall", true);
        }

        //スペースキーが押されている間
        if (Input.GetKey(KeyCode.Space) && isCharging)
        {
            animator.SetBool("IsFall", true);
            jumpCharge += Time.deltaTime;
        }

        //スペースキーが離された
        if (Input.GetKeyUp(KeyCode.Space) && isCharging && isGrounded)
        {
            isCharging = false;
            animator.SetBool("IsFall", false);
            animator.SetBool("IsJumping",true);
            Jump();
        }
    }

    //ジャンプ
    void Jump()
    {
        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpCharge / chargeTime);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isJumping = true; //ジャンプ中フラグを設定
    }

    //プレイヤーの方向
    void Flip()
    {
        //プレイヤーの移動方向に応じて向きを反転
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0 && transform.localScale.x < 0 || moveInput < 0 && transform.localScale.x > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    //着地
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = true;
            isJumping = false;
            animator.SetBool("IsFall", true);
            StartCoroutine(StopAnimationCoroutine());
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            rb.velocity = new Vector2(rb.velocity.x, maxJumpForce);
        }
    }

    //着地して0.2秒後にアニメーションをIdleにする
    IEnumerator StopAnimationCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("IsJumping",false);
        animator.SetBool("IsFall", false);
    }

    //地面から離れた
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = false;
        }
    }

    //Gizmosで地面判定の半径を視覚化する
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
