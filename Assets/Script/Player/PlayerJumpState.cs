using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //跳跃状态进入时，给予角色Y轴速度
        player.SetVelocity(rb.velocity.x, 12);
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0)
        {
            //刚体Y轴速度小于0时，角色正在下坠，切换空中状态
            stateMachine.ChangeState(player.airState);
        }

    }

    public override void Exit()
    {
        base.Exit();

    }
}
