using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAimState : PlayerState
{
    public PlayerAimState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //激活瞄准dots
        SkillManager.instance.skill_Sword.DotsActive(true);
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();

        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        if (Input.GetKeyUp(KeyCode.E))
            stateMachine.ChangeState(player.idleState);

        if ((x > player.transform.position.x && player.facingDir == -1)
            || (x < player.transform.position.x && player.facingDir == 1))
            player.Flip();
    }
}
