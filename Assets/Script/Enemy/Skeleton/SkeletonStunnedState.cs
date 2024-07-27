using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _staeMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _staeMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //������˸��Ч
        enemy.fx.InvokeRepeating("FlashBlink", 0, .1f);
        //����ѣ��ʱ��
        stateTimer = enemy.stunnedDuration;
        //ѣ�λ���
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunnedDirection.x, enemy.stunnedDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        //�ر���˸��Ч
        enemy.fx.Invoke("CancleColorChange", 0);
    }

    public override void Update()
    {
        base.Update();
        //ѣ��ʱ��������л�����״̬
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
