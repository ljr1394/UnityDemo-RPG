using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _staeMachine, string _animBoolName) : base(_enemyBase, _staeMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
    }
}
