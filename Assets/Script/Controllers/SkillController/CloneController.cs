using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    [SerializeField] protected float colorLoosingSpeed;
    protected float cloneDuration;
    protected float cloneTimer;
    protected SpriteRenderer sr;
    protected Animator anim;
    private float faceDir = 1;
    Transform closestEnemy;
    private bool canDuplicateClone;
    private float duplicateCloneProbability;
    private bool crystalInsteadOfClone;
    [SerializeField] protected Transform attackCheck;
    [SerializeField] protected float attackCheckRadius;
    private Player player;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }
    void Start()
    {
        cloneDuration = SkillManager.instance.skill_Clone.coloneDuration;
        cloneTimer = cloneDuration;
        FaceClosestTarget();
    }

    // Update is called once per frame
    void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer <= 0)
        {
            //设置克隆对象渐变消失效果
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            //克隆对象不可见时销毁对象
            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    /// <summary>
    /// 设置克隆对象的位置、朝向、是否可攻击敌人
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_cloneDuration"></param>
    /// <param name="_canBeAttack"></param>
    /// <param name="_offset"></param>
    /// <param name="_canDuplicateClone"></param>
    /// <param name="_duplicateCloneProbability"></param>
    /// <param name="_crystalInsteadOfClone"></param>
    /// <param name="_player"></param>
    public void SetClone(Transform _transform, float _cloneDuration, bool _canBeAttack, Vector3 _offset, bool _canDuplicateClone, float _duplicateCloneProbability, bool _crystalInsteadOfClone, Player _player)
    {
        cloneDuration = _cloneDuration;
        transform.position = _transform.position + _offset;
        //设置朝向
        transform.Rotate(1, _transform.rotation.y, 1);
        if (_canBeAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        canDuplicateClone = _canDuplicateClone;
        duplicateCloneProbability = _duplicateCloneProbability;
        crystalInsteadOfClone = _crystalInsteadOfClone;
        player = _player;

    }
    /// <summary>
    /// 动画触发器
    /// </summary>
    private void AnimotionTrigger()
    {
        cloneTimer = 0;
    }


    /// <summary>
    /// 攻击触发器
    /// </summary>
    private void AttackTrigger()
    {
        //获取攻击范围内所有对象
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (Collider2D collider in colliders)
        {
            //攻击范围内是否存在地方目标
            if (collider.GetComponent<Enemy>() != null)
            {

                Enemy enemy = collider.GetComponent<Enemy>();
                player.stat.DoDamage(enemy.stat);
                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < duplicateCloneProbability * 100)
                    {
                        SkillManager.instance.skill_Clone.CreateClone(enemy.transform, true, new Vector2(1.5f * faceDir, 0));
                    }
                }
            }
        }
    }



    /// <summary>
    /// 朝向最近敌人
    /// </summary>
    private void FaceClosestTarget()
    {
        //获取范围内所有对象
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius + 5);
        //初始化最近距离为正无穷数
        float closestDistance = Mathf.Infinity;
        //遍历范围内对象
        foreach (Collider2D collider in colliders)
        {
            //是否存在敌对对象
            if (collider.GetComponent<Enemy>() != null)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                //计算玩家位置到敌对位置的向量的模
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                //判断是否比closestDistance存储的最近敌人距离更近
                if (distanceToEnemy < closestDistance)
                {
                    //更新最近敌人位置坐标closestEnemy和最近敌人距离closestDistance
                    closestEnemy = enemy.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        if (closestEnemy != null)
        {
            //修改克隆对象朝向为最近的敌人
            if (closestEnemy.position.x < transform.position.x)
            {
                faceDir = -1;
                transform.Rotate(0, 180, 0);
            }

        }
    }
}
