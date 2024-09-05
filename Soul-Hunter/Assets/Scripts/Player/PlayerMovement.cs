using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // パラメータやコンポーネントの宣言
    [SerializeField] private float moveSpeed = 0f; // 移動速度
    [SerializeField] private float maxJumpForce = 0f; // 最大ジャンプ力
    [SerializeField] private float chargeTime = 0f; // チャージ時間
    [SerializeField] private float minJumpForce = 0f; // 最小ジャンプ力
    [SerializeField] private Transform groundCheck; // 地面チェック用のトランスフォーム
    [SerializeField] private float groundCheckRadius = 0f; // 地面チェックの半径
    [SerializeField] private Animator thunderAnimator; // 雷のアニメーター

    public static bool jumpAbilityFlag = false; // ダブルジャンプのアビリティ取得フラグ
    public static bool slashAbilityFlag = false; // スラッシュのアビリティ取得フラグ
    public static bool shootAbilityFlag = false; // シュートのアビリティ取得フラグ
    
    private bool isCharging = false; // ジャンプチャージ中かどうかのフラグ
    private float jumpCharge = 0f; // ジャンプチャージの量
    private Rigidbody2D rb; // Rigidbody2Dコンポーネント
    private bool isGrounded = false; // プレイヤーが地面に接触しているかどうかのフラグ
    private bool isJumping = false; // ジャンプ中かどうかのフラグ
    private Animator animator; // Animatorコンポーネント
    public bool doubleJump = false; // ダブルジャンプの有無
    private int jumpCount = 0; // 現在のジャンプ回数
    private PlayerAbilityManager abilityManager; // アビリティマネージャー
    private Ability ability; // 現在のアビリティ
    private Ability currentAbility; // 現在のアビリティ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        abilityManager = GetComponent<PlayerAbilityManager>();
    }

    void Update()
    {
        HandleJump(); // ジャンプ処理
        Move(); // 移動処理
        Flip(); // 向きの反転処理
        ChangeSlime(); // スライムの変更処理
    }

    private void ChangeSlime()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentAbility != null)
            {
                AudioM.Instance.PlaySlimeChangeSound(); // スライム変更の音を再生
                StartCoroutine(Thunder()); // 雷のエフェクトを再生
                abilityManager.DeactivateAbility(); // 現在のアビリティを無効にする
                currentAbility = null;
                animator.SetTrigger("ChangeSlime"); // スライム変更アニメーションを再生
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (jumpAbilityFlag && !(currentAbility is DoubleJumpAbility))
            {
                AudioM.Instance.PlaySlimeChangeSound(); // スライム変更の音を再生
                StartCoroutine(Thunder()); // 雷のエフェクトを再生
                Ability ability = GetComponent<DoubleJumpAbility>(); // ダブルジャンプアビリティを取得
                abilityManager.SetAbility(ability); // アビリティを設定
                currentAbility = ability;
                animator.SetTrigger("ChangeGrasshopper"); // スライム変更アニメーションを再生
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (slashAbilityFlag && !(currentAbility is SlashAbility))
            {
                AudioM.Instance.PlaySlimeChangeSound(); // スライム変更の音を再生
                StartCoroutine(Thunder()); // 雷のエフェクトを再生
                Ability ability = GetComponent<SlashAbility>(); // スラッシュアビリティを取得
                abilityManager.SetAbility(ability); // アビリティを設定
                currentAbility = ability;
                animator.SetTrigger("ChangeMantis"); // スライム変更アニメーションを再生
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (shootAbilityFlag && !(currentAbility is ShootAbility))
            {
                AudioM.Instance.PlaySlimeChangeSound(); // スライム変更の音を再生
                StartCoroutine(Thunder()); // 雷のエフェクトを再生
                Ability ability = GetComponent<ShootAbility>(); // シュートアビリティを取得
                abilityManager.SetAbility(ability); // アビリティを設定
                currentAbility = ability;
                animator.SetTrigger("ChangePheropsophus"); // スライム変更アニメーションを再生
            }
        }
    }

    void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground")); // 地面チェック

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            jumpCharge = 0f;

            // 現在のアビリティに応じてアニメーションを設定
            if (currentAbility is DoubleJumpAbility)
            {
                animator.SetBool("GrasshopperIsFall", true);
            }
            else if (currentAbility is SlashAbility)
            {
                animator.SetBool("MantisIsFall", true);
            }
            else if (currentAbility is ShootAbility)
            {
                animator.SetBool("PheropsophusIsFall", true);
            }
            else
            {
                animator.SetBool("IsFall", true);
            }
        }

        if (Input.GetKey(KeyCode.Space) && isCharging)
        {
            // 現在のアビリティに応じてアニメーションを設定
            if (currentAbility is DoubleJumpAbility)
            {
                animator.SetBool("GrasshopperIsFall", true);
            }
            else if (currentAbility is SlashAbility)
            {
                animator.SetBool("MantisIsFall", true);
            }
            else if (currentAbility is ShootAbility)
            {
                animator.SetBool("PheropsophusIsFall", true);
            }
            else
            {
                animator.SetBool("IsFall", true);
            }
            
            jumpCharge += Time.deltaTime; // チャージ時間の更新
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            isCharging = false;

            // 現在のアビリティに応じてアニメーションを設定
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
            else if (currentAbility is ShootAbility)
            {
                animator.SetBool("PheropsophusIsFall", false);
                animator.SetBool("PheropsophusIsJumping", true);
            }
            else
            {
                animator.SetBool("IsFall", false);
                animator.SetBool("IsJumping", true);
            }

            // ダブルジャンプの処理を追加
            if (isGrounded || (doubleJump && jumpCount < 2) && currentAbility is DoubleJumpAbility)
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
            float moveInput = Input.GetAxis("Horizontal"); // 水平方向の入力
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y); // 移動処理
        }
    }

    void Jump()
    {
        AudioM.Instance.PlayJumpSound(); // ジャンプの音を再生
        rb.velocity = Vector3.zero; // 現在の速度をリセット
        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpCharge / chargeTime); // ジャンプ力を計算
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // ジャンプ力を適用
        isJumping = true;
        jumpCharge = 0f;
    }

    void Flip()
    {
        float moveInput = Input.GetAxis("Horizontal"); // 水平方向の入力
        if (moveInput > 0 && transform.localScale.x < 0 || moveInput < 0 && transform.localScale.x > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1; // 向きを反転
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

            // 現在のアビリティに応じてアニメーションを設定
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
            else if (currentAbility is ShootAbility)
            {
                animator.SetBool("PheropsophusIsJumping", false);
                animator.SetBool("PheropsophusIsFall", false);
            }
            else
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFall", false);
            }
        }
        else if (collision.gameObject.CompareTag("JumpAbility")) // アビリティ取得の処理
        {
            jumpAbilityFlag = true;
            Ability ability = GetComponent<DoubleJumpAbility>();

            if (ability != null)
            {
                AudioM.Instance.PlaySlimeChangeSound(); // スライム変更の音を再生
                StartCoroutine(Thunder()); // 雷のエフェクトを再生
                abilityManager.SetAbility(ability); // アビリティを設定
                currentAbility = ability; // currentAbilityに設定
                Destroy(collision.gameObject); // コリジョン対象を破壊
            }
            animator.SetTrigger("ChangeGrasshopper"); // スライム変更アニメーションを再生
        }
        else if (collision.gameObject.CompareTag("SlashAbility"))
        {
            slashAbilityFlag = true;
            Ability ability = GetComponent<SlashAbility>();

            if (ability != null)
            {
                AudioM.Instance.PlaySlimeChangeSound(); // スライム変更の音を再生
                StartCoroutine(Thunder()); // 雷のエフェクトを再生
                abilityManager.SetAbility(ability); // アビリティを設定
                currentAbility = ability; // currentAbilityに設定
                Destroy(collision.gameObject); // コリジョン対象を破壊
            }
            animator.SetTrigger("ChangeMantis"); // スライム変更アニメーションを再生
        }
        else if (collision.gameObject.CompareTag("ShootAbility"))
        {
            shootAbilityFlag = true;
            Ability ability = GetComponent<ShootAbility>();

            if (ability != null)
            {
                AudioM.Instance.PlaySlimeChangeSound(); // スライム変更の音を再生
                StartCoroutine(Thunder()); // 雷のエフェクトを再生
                abilityManager.SetAbility(ability); // アビリティを設定
                currentAbility = ability; // currentAbilityに設定
                Destroy(collision.gameObject); // コリジョン対象を破壊
            }
            animator.SetTrigger("ChangePheropsophus"); // スライム変更アニメーションを再生
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            rb.velocity = new Vector2(rb.velocity.x, maxJumpForce); // 敵と接触した際にジャンプ力を最大にする
            jumpCount = 1; // ジャンプ回数をリセット
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red; // Gizmosの色を設定
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius); // GroundCheckの位置と範囲を描画
        }
    }

    IEnumerator Thunder()
    {
        thunderAnimator.SetBool("Thunder", true); // 雷アニメーションを開始
        yield return new WaitForSeconds(0.1f); // 0.1秒待機
        thunderAnimator.SetBool("Thunder", false); // 雷アニメーションを終了
    }
}
