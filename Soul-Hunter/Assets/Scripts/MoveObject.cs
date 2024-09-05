using UnityEngine;
using DG.Tweening;

public class MoveObject : MonoBehaviour
{
    // 移動方向を指定する列挙型
    public enum Direction
    {
        Horizontal, // 水平方向
        Vertical,   // 垂直方向
        DiagonalUp, // 斜め上方向
        DiagonalDown // 斜め下方向
    }

    [SerializeField] private Direction moveDirection = Direction.Horizontal; // 移動方向の選択
    [SerializeField] private float moveDistance = 5f; // 移動距離
    [SerializeField] private float moveDuration = 2f; // 移動にかかる時間
    private Tween moveTween; // Tween変数を保持

    private void Start()
    {
        Vector3 originalPosition = transform.position; // 現在の位置を保存
        Vector3 targetPosition = originalPosition; // 移動先の位置を初期化

        // 移動方向に応じてターゲット位置を設定
        switch (moveDirection)
        {
            case Direction.Horizontal:
                targetPosition.x += moveDistance; // 水平方向に移動
                break;
            case Direction.Vertical:
                targetPosition.y += moveDistance; // 垂直方向に移動
                break;
            case Direction.DiagonalUp:
                targetPosition.x += moveDistance; // 斜め上方向に移動
                targetPosition.y += moveDistance;
                break;
            case Direction.DiagonalDown:
                targetPosition.x += moveDistance; // 斜め下方向に移動
                targetPosition.y -= moveDistance;
                break;
        }

        // 移動アニメーションを設定し、Tween変数に格納
        moveTween = transform.DOMove(targetPosition, moveDuration)
            .SetEase(Ease.InOutSine) // イージングの設定
            .SetLoops(-1, LoopType.Yoyo); // ループ設定
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーが衝突したとき、プレイヤーをこのオブジェクトの子にする
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーが衝突から外れたとき、親を解除する
            collision.transform.SetParent(null);
        }
    }

    private void OnDestroy()
    {
        // オブジェクトが破棄されたときにTweenを停止
        if (moveTween != null)
        {
            moveTween.Kill();
        }
    }
}
