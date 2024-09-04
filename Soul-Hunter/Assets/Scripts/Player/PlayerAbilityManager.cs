using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private Ability currentAbility;
    private float chargeTime = 0f;  // チャージ時間
    [SerializeField] private float maxChargeTime = 3.0f; // 最大チャージ時間

    void Update()
    {
        if (currentAbility is SlashAbility)
        {
            if (Input.GetKeyDown(KeyCode.F))// 'F'キーで弾を発射
            {
                currentAbility.Activate(GetComponent<PlayerMovement>());
            }
        }

        if (currentAbility is ShootAbility)
        {
            Animator animator = GetComponent<Animator>();

            if (Input.GetKeyDown(KeyCode.F)) // 'F'キーを押したとき
            {
                animator.SetBool("PheropsophusIsCharge", true);
                chargeTime = 0f; // チャージ時間をリセット
            }

            if (Input.GetKey(KeyCode.F)) // 'F'キーを押し続けているとき
            {
                chargeTime += Time.deltaTime; // チャージ時間を増加
                chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime); // チャージ時間を制限
            }

            if (Input.GetKeyUp(KeyCode.F)) // 'F'キーを離したとき
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

    public void DeactivateAbility()
    {
        if (currentAbility != null)
        {
            currentAbility.Deactivate(GetComponent<PlayerMovement>());
        }
    }
    
    public void SetAbility(Ability newAbility)
    {
        if (currentAbility != null)
        {
            currentAbility.Deactivate(GetComponent<PlayerMovement>());
        }

        currentAbility = newAbility;
        

        if (currentAbility is DoubleJumpAbility)
        {
            currentAbility.Activate(GetComponent<PlayerMovement>());
        }
    }
}
