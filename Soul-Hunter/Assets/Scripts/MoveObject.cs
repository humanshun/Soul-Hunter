using UnityEngine;
using DG.Tweening;

public class MoveObject : MonoBehaviour
{
    public enum Direction
    {
        Horizontal,
        Vertical,
        DiagonalUp,
        DiagonalDown
    }

    [SerializeField] private Direction moveDirection = Direction.Horizontal; // 移動方向の選択
    [SerializeField] private float moveDistance = 5f; // 移動距離
    [SerializeField] private float moveDuration = 2f; // 移動にかかる時間
    private Tween moveTween; // Tween変数を保持

    private void Start()
    {
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition;

        // 移動方向に応じてターゲット位置を設定
        switch (moveDirection)
        {
            case Direction.Horizontal:
                targetPosition.x += moveDistance;
                break;
            case Direction.Vertical:
                targetPosition.y += moveDistance;
                break;
            case Direction.DiagonalUp:
                targetPosition.x += moveDistance;
                targetPosition.y += moveDistance;
                break;
            case Direction.DiagonalDown:
                targetPosition.x += moveDistance;
                targetPosition.y -= moveDistance;
                break;
        }

        // 移動アニメーションを設定し、Tween変数に格納
        moveTween = transform.DOMove(targetPosition, moveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // トリガーされたオブジェクトを子オブジェクトにする
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // トリガーから外れたオブジェクトの親を解除する
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
