
using UnityEngine;
public class PlayerIdleState : PlayerGroundState
{

    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //在X虚拟轴上有输入
        if ((inputX != 0 && !player.IsWallDetected()) || inputX == -player.facingDir)
            if (!player.isBusy)
                //切换为移动状态
                stateMachine.ChangeState(player.moveState);
    }
}
