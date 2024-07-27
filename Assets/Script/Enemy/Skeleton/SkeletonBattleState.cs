using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    protected Enemy_Skeleton enemy;
    private Transform player;
    private float moveDir;
    private bool isClose;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _staeMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _staeMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;


    }
    public override void Update()
    {
        base.Update();


        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.attackDistance)
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
                if (!isClose)
                {
                    SetClose(true);

                }
            }
            else if (isClose)
            {
                SetClose(false);
            }

        }
        else if (stateTimer < 0 || Vector2.Distance(enemy.transform.position, player.transform.position) > 15)
            stateMachine.ChangeState(enemy.idleState);



        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }

        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }




        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        if (isClose)
        {
            enemy.ZeroVelocity();
        }

    }
    public override void Exit()
    {
        base.Exit();

    }

    public bool CanAttack()
    {

        return Time.time > enemy.lastTimeAttacked + enemy.attackCooling;
    }

    public void SetClose(bool _isClose)
    {
        isClose = _isClose;
        enemy.anim.SetBool("IsClose", isClose);
    }


}
