using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    // 能力をアクティブにするメソッド
    public abstract void Activate(PlayerMovement player);

    // 能力を非アクティブにするメソッド
    public abstract void Deactivate(PlayerMovement player);
}
