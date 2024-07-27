using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyState
{
    public Enemy enemyBase;
    public EnemyStateMachine stateMachine;
    public string animBoolName;
    public Rigidbody2D rb;
    public float stateTimer;
    public bool triggerCalled;
    public bool isCloseToPlayer;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _staeMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _staeMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        enemyBase.anim.SetBool(animBoolName, true);
        rb = enemyBase.GetComponent<Rigidbody2D>();
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimotionFinishTrigger()
    {
        triggerCalled = true;
    }


}
