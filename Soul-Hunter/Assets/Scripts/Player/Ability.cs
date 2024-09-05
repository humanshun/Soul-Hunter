using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    // 能力をアクティブにするメソッド（具体的な実装はサブクラスで行う）
    public abstract void Activate(PlayerMovement player);

    // 能力を非アクティブにするメソッド（具体的な実装はサブクラスで行う）
    public abstract void Deactivate(PlayerMovement player);
}
