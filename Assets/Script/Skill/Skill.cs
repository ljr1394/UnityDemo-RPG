using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float cooldownTimer;
    public Player player { get; private set; }

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 技能是否可用
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        //冷却时间是否满足
        if (cooldownTimer < 0)
        {
            UseSkill();
            //更新冷却时间器
            cooldownTimer = coolDown;
            return true;
        }
        return false;
    }
    public virtual void UseSkill()
    {

    }

    public virtual void SkillComplish()
    {

    }


}
