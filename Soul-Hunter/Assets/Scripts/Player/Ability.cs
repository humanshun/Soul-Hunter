using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public abstract void Activate(PlayerMovement player);
    public abstract void Deactivate(PlayerMovement player); // Deactivateメソッドの定義
}
