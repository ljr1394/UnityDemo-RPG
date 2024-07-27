using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;

public class Skill_Clone : Skill
{
    [Header("Clone Info")]
    [SerializeField] protected GameObject clonePrefab;
    [SerializeField] public float coloneDuration;
    [SerializeField] protected bool canBeAttack;
    [SerializeField] protected bool createCloneOnDashStart;
    [SerializeField] protected bool createCloneOnDashEnd;
    [SerializeField] protected bool createCloneOnCounterAttack;
    [SerializeField] protected bool canDuplicateClone;
    [SerializeField] protected float duplicateCloneProbability;
    public bool crystalInsteadOfClone;

    /// <summary>
    /// 创建克隆对象
    /// </summary>
    /// <param name="transform">克隆对象初始位置</param>
    public void CreateClone(Transform transform)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.skill_Crystal.CreateCrystal();
            return;
        }
        //实例化克隆对象
        GameObject cloneGo = Instantiate(clonePrefab);
        //初始化克隆对象
        cloneGo.GetComponent<CloneController>().SetClone(transform, coloneDuration, canBeAttack, Vector3.zero, canDuplicateClone, duplicateCloneProbability, crystalInsteadOfClone, player);

    }
    /// <summary>
    /// 创建克隆对象
    /// </summary>
    /// <param name="transform">克隆对象位置</param>
    /// <param name="_canBeAttack">是否可以攻击</param>
    /// <param name="_offset">位置偏移量</param>
    public void CreateClone(Transform transform, bool _canBeAttack, Vector3 _offset = default)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.skill_Crystal.CreateCrystal();
            return;
        }
        //实例化克隆对象
        GameObject cloneGo = Instantiate(clonePrefab);
        //初始化克隆对象
        cloneGo.GetComponent<CloneController>().SetClone(transform, coloneDuration, _canBeAttack, _offset, canDuplicateClone, duplicateCloneProbability, crystalInsteadOfClone, player);

    }

    public void CreateCloneOnDashStart(Transform transform)
    {
        if (createCloneOnDashStart)
        {
            CreateClone(transform);
        }
    }

    public void CreateCloneOnDashEnd(Transform transform)
    {
        if (createCloneOnDashEnd)
        {
            CreateClone(transform);
        }
    }

    public void CreateCloneOnCounterAttack(Transform transform)
    {
        if (createCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(transform, new Vector2(2 * player.facingDir, 0)));
        }
    }

    public IEnumerator CreateCloneWithDelay(Transform transform, Vector2 _offSet)
    {
        yield return new WaitForSeconds(0.3f);
        CreateClone(transform, true, _offSet);
    }


}
