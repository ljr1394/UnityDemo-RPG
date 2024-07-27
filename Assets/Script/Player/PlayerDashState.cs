using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
        player.skillManager.skill_Clone.CreateCloneOnDashStart(player.transform);

    }

    public override void Update()
    {
        base.Update();
        //空中冲刺时会附带下坠（仍然附带Y速度）
        //player.SetVelocity(player.dashSpeed * player.facingDir, rb.velocity.y);
        if (player.IsWallDetected() && !player.IsGroundDetected())
            stateMachine.ChangeState(player.wallSlide);
        //空中冲刺时滞空
        player.SetVelocity(player.dashSpeed * player.facingDir, 0);
        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

    }

    public override void Exit()
    {
        base.Exit();
        player.skillManager.skill_Clone.CreateCloneOnDashEnd(player.transform);
        player.SetVelocity(0, rb.velocity.y);
    }
}
