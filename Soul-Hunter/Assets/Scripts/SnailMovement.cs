using UnityEngine;

public class SnailMovement : WormMovement
{
    // 追加のカスタムコードをここに記述します

    protected override void Start()
    {
        base.Start();
        // 必要に応じて開始時の初期化を追加
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // 必要に応じてフレームごとの処理を追加
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        // 必要に応じてトリガーに当たったときの処理を追加
    }
}
