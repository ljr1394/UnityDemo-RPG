using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();

    #region 水晶基本属性
    private float crystalDuration;
    private float moveSpeed;
    private float moveRadius;
    #endregion 

    #region 水晶扩展技能
    private bool canBeExplode;
    private bool canBeMove;
    #endregion

    #region 水晶状态
    private bool isExplode;
    private bool isRandom;
    #endregion

    #region 定时器
    private float crystalTimer;
    #endregion
    public LayerMask whatIsEnemy;
    private Transform target;

    private Player player;

    void Start()
    {
        //初始化水晶存活时间
        crystalTimer = crystalDuration;
    }

    void Update()
    {
        crystalTimer -= Time.deltaTime;
        //判断是否可移动且未爆炸
        if (canBeMove && !isExplode)
        {

            if (!isRandom)
            {
                //设置移动目标点
                target = TargetTransform();
            }


            //移动目标点不为空
            if (target != null)
            {
                //向目标点移动
                transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);
            }

        }
        //剩余存活时间为0
        if (crystalTimer < 0)
        {
            //完成水晶技能
            FinishCrystal();
        }

    }

    public void RandomChooseTarget(float _radius)
    {
        isRandom = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius, whatIsEnemy);
        if (colliders.Length > 0)
        {

            int index = Random.Range(0, colliders.Length);
            Debug.Log(colliders[index].transform);
            target = colliders[index].transform;
        }
    }
    public void FinishCrystal()
    {
        //水晶可爆炸
        if (canBeExplode)
        {
            //切换为爆炸状态
            isExplode = true;
            //播放爆炸动画
            anim.SetBool("Explode", true);

        }
        else
        {
            //水晶销毁
            SelfDestroy();
        }
    }

    /// <summary>
    /// 初始化水晶属性
    /// </summary>
    /// <param name="_crystalDuration"></param>
    /// <param name="_transform"></param>
    /// <param name="_canBeExplode"></param>
    /// <param name="_canBeMove"></param>
    /// <param name="_moveRadius"></param>
    /// <param name="_moveSpeed"></param>
    public void SetupCrystal(float _crystalDuration, Transform _transform, bool _canBeExplode, bool _canBeMove, float _moveRadius, float _moveSpeed, Player _player)
    {
        transform.position = _transform.position;
        crystalDuration = _crystalDuration;
        canBeExplode = _canBeExplode;
        canBeMove = _canBeMove;
        moveRadius = _moveRadius;
        moveSpeed = _moveSpeed;
        player = _player;
    }

    /// <summary>
    /// 爆炸触发器
    /// </summary>
    private void ExplodeTrigger()
    {
        //获取爆炸范围内的地方单位
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                player.stat.DoMagicDamage(collider.GetComponent<Enemy>().stat);
            }
        }

    }
    /// <summary>
    /// 爆炸动画完毕触发器
    /// </summary>
    private void FinishExplodeTrigger() => SelfDestroy();//销毁水晶


    /// <summary>
    /// 销毁水晶
    /// </summary>
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 获取最近敌人坐标
    /// </summary>
    /// <returns></returns>
    private Transform TargetTransform()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, moveRadius);
        float i = moveRadius;
        Transform target = null;
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < i)
                {
                    i = distance;
                    target = collider.transform;
                }
            }

        }
        return target;

    }

    /// <summary>
    /// 接触敌人时爆炸
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() && canBeExplode)
            FinishCrystal();


    }
}
