using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    #region 状态
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }
    public SkeletonStunnedState stuunedState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Battle", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stuunedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SkeletonDeadState(this, stateMachine, "Dead");
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 骷髅是否被眩晕，并切换眩晕状态
    /// </summary>
    /// <returns>是否被眩晕</returns>
    public override bool CanBeStunned()
    {
        //是否被眩晕
        if (base.CanBeStunned())
        {
            //切换眩晕状态
            stateMachine.ChangeState(stuunedState);
            return true;
        }
        return false;

    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
