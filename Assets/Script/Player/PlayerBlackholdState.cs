using UnityEngine;

public class PlayerBlackholdState : PlayerState
{

    private float flyTime = .4f;
    private bool useSkill;
    private float defaultGravity;

    private Skill_Blackhold skillBlackhold;
    public PlayerBlackholdState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        useSkill = false;
        stateTimer = flyTime;
        skillBlackhold = SkillManager.instance.skill_Blackhold;
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }
    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
        player.SetVisible(true);
    }
    public override void Update()
    {
        stateTimer -= Time.deltaTime;
        //起飞中
        if (stateTimer > 0)
        {
            //起飞
            player.SetVelocity(0, 15);
        }
        //起飞完毕
        if (stateTimer < 0)
        {

            //缓慢降落
            player.SetVelocity(0, -.15f);
            //是否是使用技能状态
            if (!useSkill)
            {
                //判断是否可使用技能
                if (skillBlackhold.CanUseSkill())
                {
                    //使用技能
                    skillBlackhold.UseSkill();
                    //设置为使用技能状态
                    useSkill = true;

                }

            }



        }
        if (skillBlackhold.IsComplish())
            SkillCompolish();




    }
    public void SkillCompolish()
    {
        skillBlackhold.SkillComplish();
        stateMachine.ChangeState(player.airState);
    }


}
