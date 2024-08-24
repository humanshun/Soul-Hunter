using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpAbility : Ability
{
    public override void Activate(PlayerMovement player)
    {
        player.doubleJump = true;  // ダブルジャンプを有効にする
    }

    public override void Deactivate(PlayerMovement player)
    {
        player.doubleJump = false; // ダブルジャンプを無効にする
    }
}

