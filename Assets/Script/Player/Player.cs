using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : Entity
{

    #region 状态
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimeryAttack primeryAttack { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimState aimState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholdState blackholdState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion



    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration;

    [Header("Move info")]
    public float jumpForce = 10f;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashSpeed = 25f;
    private float defaultDashSpeed;
    public float dashDuration = 0.2f;
    public float dashDir { get; private set; }

    public bool isBusy { get; private set; } = false;
    public SkillManager skillManager;
    public GameObject sword;




    protected override void Awake()
    {
        base.Awake();
        //初始化对象
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primeryAttack = new PlayerPrimeryAttack(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimState = new PlayerAimState(this, stateMachine, "Aim");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "Catch");
        blackholdState = new PlayerBlackholdState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Dead");
    }
    protected override void Start()
    {
        base.Start();
        //初始化角色状态机
        skillManager = SkillManager.instance;
        defaultDashSpeed = dashSpeed;
        defaultJumpForce = jumpForce;
        stateMachine.Initialize(idleState);

    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();

    }

    /// <summary>
    /// 检查玩家是否进行冲刺
    /// </summary>
    public void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.skill_Dash.CanUseSkill())
        {
            //冲刺面向取决于虚拟X轴的方向
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir != 0)
            {
                stateMachine.ChangeState(dashState);
            }



        }
    }
    public override void SlowEntityBy(float _slowPercentage, float _duration)
    {
        jumpForce *= 1 - _slowPercentage;
        dashSpeed *= 1 - _slowPercentage;
        base.SlowEntityBy(_slowPercentage, _duration);
    }

    public override void ReturnDefaultSpeed()
    {
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
        base.ReturnDefaultSpeed();
    }
    public IEnumerator BusyFor(float seconds)
    {

        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

    public void AnimotionTrigger() => stateMachine.currentState.AnimotionFinishTrigger();

    public void ComboTrigger() => stateMachine.currentState.AnimotionComboTrigger();

    public void AssginNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    /// <summary>
    /// 玩家抓取剑
    /// </summary>
    public void CatchTheSword()
    {
        //销毁剑对象
        Destroy(sword);
        //切换为抓取剑状态
        stateMachine.ChangeState(catchSwordState);

    }

    public void TransitionToIdle() => stateMachine.ChangeState(idleState);


    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }



}
