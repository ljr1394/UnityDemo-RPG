using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;
        if ((sword.position.x > player.transform.position.x && player.facingDir == -1)
            || (sword.position.x < player.transform.position.x && player.facingDir == 1))
            player.Flip();
        player.StartCoroutine("BusyFor", 0.2f);
        rb.velocity = new Vector2(-player.facingDir * 7, 0);
    }
    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
