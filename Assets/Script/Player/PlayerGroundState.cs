using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        if (Input.GetKeyDown(KeyCode.E) && !hasSword())
            stateMachine.ChangeState(player.aimState);

        //角色在地面按Q切换格挡状态
        if (Input.GetKeyDown(KeyCode.Q))
            stateMachine.ChangeState(player.counterAttackState);

        if (Input.GetKeyDown(KeyCode.J))
            stateMachine.ChangeState(player.primeryAttack);
        if (!player.IsGroundDetected())
            //角色不在地面切换空中状态
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            //角色在地面且按下跳跃快捷键时切换跳跃状态
            stateMachine.ChangeState(player.jumpState);


    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool hasSword()
    {
        if (player.sword)
        {
            player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
            return true;
        }
        return false;


    }
}
