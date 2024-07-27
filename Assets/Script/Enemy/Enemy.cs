using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned Info")]
    public float stunnedDuration;
    public Vector2 stunnedDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImg;

    [Header("Move Info")]

    public float idleTime = 2f;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooling;
    public float battleTime;
    [HideInInspector] public float lastTimeAttacked;
    public float level;
    public EnemyStateMachine stateMachine { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

    }

    public void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
            return;
        }
        moveSpeed = defaultMoveSpeed;
        anim.speed = 1;

    }

    public virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }
    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    public virtual void AnimotionFinishTrigger() => stateMachine.currentState.AnimotionFinishTrigger();

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImg.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImg.SetActive(false);
    }


    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

}
