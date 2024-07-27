using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }



    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        //给刚体设置速度进行移动
        player.SetVelocity(inputX * player.moveSpeed, rb.velocity.y);
        if (inputX == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
    }

}
