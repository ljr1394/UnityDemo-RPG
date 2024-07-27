using System.Collections.Generic;
using UnityEngine;

public class Skill_Crystal : Skill
{
    [Header("Crystal Info")]
    [SerializeField] protected GameObject crystalPrefab;
    [SerializeField] protected float crystalDuration;
    [SerializeField] protected bool cloneInsteadOfCrystal;

    [Header("Explode Info")]
    [SerializeField] bool canBeExplode;

    [Header("Crystal Move Info")]
    [SerializeField] private bool canBeMove;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRadius;

    [Header("Multi Crystal")]
    [SerializeField] bool canUseMultiStacks;
    [SerializeField] float amountOfCrystal;
    [SerializeField] private float abilityInterval;


    private float abilityTimer;

    public GameObject currentCrystal { get; private set; }

    private List<GameObject> crystals = new List<GameObject>();


    protected override void Update()
    {

        //判断水晶是否可充能
        if (canUseMultiStacks)
        {
            //释放间隔定时器
            abilityTimer -= Time.deltaTime;
            //判断是否达到充能上限且充能时间小于0
            if (crystals.Count < amountOfCrystal && cooldownTimer < 0)
            {
                //充能水晶
                crystals.Add(crystalPrefab);
                //刷新充能时间
                cooldownTimer = coolDown;
            }
            //充能到达上限
            if (crystals.Count == amountOfCrystal)
                return;
        }
        cooldownTimer -= Time.deltaTime;


    }

    /// <summary>
    /// 是否可使用技能
    /// </summary>
    /// <returns></returns>
    public override bool CanUseSkill()
    {
        //判断是否可充能
        if (canUseMultiStacks)
        {
            //如果充能次数不为0且满足释放间隔
            if (crystals.Count > 0 && abilityTimer < 0)
            {
                //重置释放间隔
                abilityTimer = abilityInterval;
                //使用技能
                UseSkill();
                return true;
            }
            return false;


        }
        if (currentCrystal && !canBeMove)
        {
            UseSkill();
            return true;
        }
        return base.CanUseSkill();
    }

    /// <summary>
    /// 使用技能
    /// </summary>
    public override void UseSkill()
    {
        base.UseSkill();
        //是否可使用多颗水晶
        if (CanUseMultiCrystal())
        {
            return;
        }

        if (canBeMove)
        {
            CreateCrystal(crystalPrefab);
            return;
        }
        //判断当前是否已有生成水晶
        if (currentCrystal == null)
        {
            //实例化水晶
            CreateCrystal(crystalPrefab);

        }
        else
        {

            //当前已生成水晶，且再次释放技能，人物与水晶进行换位
            Vector2 playerPosisition = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPosisition;
            if (cloneInsteadOfCrystal)
            {
                player.skillManager.skill_Clone.CreateClone(currentCrystal.transform, true, Vector2.zero);

            }

            currentCrystal.GetComponent<Crystal_Skill_Controller>().FinishCrystal();

        }




    }

    /// <summary>
    /// 是否可使用多颗水晶
    /// </summary>
    /// <returns></returns>
    public bool CanUseMultiCrystal()
    {
        //判断可否使用多颗水晶且当前已充能水晶不为0
        if (canUseMultiStacks && crystals.Count > 0)
        {
            //实例化水晶
            CreateCrystal(crystals[crystals.Count - 1]);
            //充能层数-1
            crystals.RemoveAt(crystals.Count - 1);
        }
        return false;



    }
    /// <summary>
    /// 实例化水晶
    /// </summary>
    /// <param name="crystalPrefab"></param>
    public void CreateCrystal(GameObject _crystalPrefab)
    {

        GameObject crystal = Instantiate(_crystalPrefab);
        crystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration, player.transform, canBeExplode, canBeMove, moveRadius, moveSpeed, player);
        currentCrystal = crystal;
    }

    public void CreateCrystal()
    {

        GameObject crystal = Instantiate(crystalPrefab);
        crystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration, player.transform, canBeExplode, canBeMove, moveRadius, moveSpeed, player);
        currentCrystal = crystal;
    }



}









