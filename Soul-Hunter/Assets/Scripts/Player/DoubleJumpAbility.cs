using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpAbility : Ability
{
    // ダブルジャンプを有効にする
    public override void Activate(PlayerMovement player)
    {
        player.doubleJump = true;
    }

    // ダブルジャンプを無効にする
    public override void Deactivate(PlayerMovement player)
    {
        player.doubleJump = false;
    }
}
