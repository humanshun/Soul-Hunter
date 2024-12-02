using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private Ability currentAbility;
    private float chargeTime = 0f;  // チャージ時間
    [SerializeField] private float maxChargeTime = 3.0f; // 最大チャージ時間

    void Update()
    {
        // 現在のアビリティがSlashAbilityの場合
        if (currentAbility is SlashAbility)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentAbility.Activate(GetComponent<PlayerMovement>());
            }
        }

        // 現在のアビリティがShootAbilityの場合
        if (currentAbility is ShootAbility)
        {
            Animator animator = GetComponent<Animator>();

            if (Input.GetKeyDown(KeyCode.F))
            {
                animator.SetBool("PheropsophusIsCharge", true);
                chargeTime = 0f; // チャージ時間をリセット
            }

            if (Input.GetKey(KeyCode.F))
            {
                chargeTime += Time.deltaTime; // チャージ時間を増加
                chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime); // チャージ時間を制限
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                if (currentAbility is ShootAbility shootAbility)
                {
                    animator.SetBool("PheropsophusIsCharge", false);
                    animator.SetBool("PheropsophusIsAttack", true);
                    float normalizedChargeTime = chargeTime / maxChargeTime; // チャージ時間を正規化
                    shootAbility.Shoot(GetComponent<PlayerMovement>(), normalizedChargeTime); // 正規化されたチャージ時間を渡して発射
                }
            }
        }
    }

    // アビリティを無効にする
    public void DeactivateAbility()
    {
        if (currentAbility != null)
        {
            currentAbility.Deactivate(GetComponent<PlayerMovement>());
        }
    }
    
    // 新しいアビリティを設定する
    public void SetAbility(Ability newAbility)
    {
        // 現在のアビリティを無効にする
        if (currentAbility != null)
        {
            currentAbility.Deactivate(GetComponent<PlayerMovement>());
        }

        currentAbility = newAbility;

        // 新しいアビリティがDoubleJumpAbilityの場合は即座にアクティブにする
        if (currentAbility is DoubleJumpAbility)
        {
            currentAbility.Activate(GetComponent<PlayerMovement>());
        }
    }
}
