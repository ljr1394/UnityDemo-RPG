using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Blackhold : Skill
{
    [Header("blackhold Info")]
    [SerializeField] private GameObject blackholdPrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackholdDuration;
    [SerializeField] private float amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    private float vanishTimer = 1f;
    private GameObject currentBlackhold;

    /// <summary>
    /// 是否可使用技能
    /// </summary>
    /// <returns></returns>
    public override bool CanUseSkill()
    {
        return cooldownTimer < 0;
    }

    /// <summary>
    /// 使用技能
    /// </summary>
    public override void UseSkill()
    {
        base.UseSkill();
        //创建黑洞
        GameObject blackhold = Instantiate(blackholdPrefab, player.transform.position, Quaternion.identity);
        currentBlackhold = blackhold;
        Blackhold_Skill_Controller blackholdScript = currentBlackhold.GetComponent<Blackhold_Skill_Controller>();
        //初始化黑洞
        blackholdScript.SetupBlackhold(player.transform.position, maxSize, growSpeed, blackholdDuration, amountOfAttacks, cloneCooldown, shrinkSpeed);

    }
    /// <summary>
    /// 技能使用完毕时进入冷却
    /// </summary>
    public override void SkillComplish()
    {
        if (currentBlackhold != null && IsComplish())
        {

            Destroy(currentBlackhold);
            currentBlackhold = null;
            cooldownTimer = coolDown;
        }


    }
    public bool IsComplish()
    {
        if (currentBlackhold == null)
            return false;
        return currentBlackhold.GetComponent<Blackhold_Skill_Controller>().isComplish;

    }






}
