using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private Ability currentAbility;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // 'F'キーで弾を発射
        {
            if (currentAbility != null && currentAbility is ProjectileAbility)
            {
                currentAbility.Activate(GetComponent<PlayerMovement>());
            }
        }
    }

    public void SetAbility(Ability newAbility)
    {
        if (currentAbility != null)
        {
            currentAbility.Deactivate(GetComponent<PlayerMovement>());
        }

        currentAbility = newAbility;
        // ここでアビリティを即座に発動する場合
        // currentAbility.Activate(GetComponent<PlayerMovement>());
    }
}
