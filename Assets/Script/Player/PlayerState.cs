using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected float inputX;
    protected float inputY;

    protected Rigidbody2D rb;
    public PlayerStateMachine stateMachine;

    public Player player;

    public string animBoolName;

    protected float stateTimer;

    protected bool triggerCalled;
    protected bool comboTrigger;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        stateMachine = _stateMachine;
        player = _player;
        animBoolName = _animBoolName;

    }
    /// <summary>
    /// 进入状态调用函数
    /// </summary>
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
        comboTrigger = false;
    }
    /// <summary>
    /// 当前状态更新函数
    /// </summary>
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        //监控角色刚体Y轴速度，根据不同速度播放跳跃或下坠动画
        player.anim.SetFloat("yVelocity", rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.R) && player.skillManager.skill_Blackhold.CanUseSkill())
            stateMachine.ChangeState(player.blackholdState);

        if (Input.GetKeyDown(KeyCode.K))
            player.skillManager.skill_Crystal.CanUseSkill();


    }
    /// <summary>
    /// 退出状态调用函数
    /// </summary>
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimotionFinishTrigger()
    {

        triggerCalled = true;
    }

    public void AnimotionComboTrigger()
    {

        comboTrigger = true;
    }
}
