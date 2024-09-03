using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private Ability currentAbility;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // 'F'キーで弾を発射
        {
            if (currentAbility is ShootAbility || currentAbility is SlashAbility)
            {
                currentAbility.Activate(GetComponent<PlayerMovement>());
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
        else{}
    }
}
