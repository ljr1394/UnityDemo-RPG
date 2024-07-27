using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Sword_Skill_Controller : MonoBehaviour
{
    #region 组件
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    #endregion

    #region 旋转
    public bool canRotate { get; private set; } = true;
    private bool isReturn;
    #endregion

    [Header("Return Info")]
    private float returnSpeed;


    [Header("Bounce Info")]
    private float bouncingSpeed;
    private bool isBouncing;
    private float amountOfBounce;
    private List<GameObject> enemyTarget;
    private int targetIndex;
    private float bounceFreezeTime;


    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool isSpinning;
    private bool wasStopped;
    private Vector2 throwPosition;
    private float hitTimer;
    private float hitCooldown;


    [Header("pierce Info")]
    private float pierceAmount;

    private float freezeTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        cd = GetComponent<CircleCollider2D>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("Flip", canRotate);
    }

    private void Update()
    {
        //剑是否可旋转
        if (canRotate)
            //剑的方向与速度向量一致
            transform.right = rb.velocity * Time.deltaTime;

        //剑返回玩家位置
        if (isReturn)
        {
            //向玩家位置移动
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        Bouncing();
        Spinning();
    }

    /// <summary>
    /// 旋转特性技能逻辑
    /// </summary>
    private void Spinning()
    {
        if (isSpinning)
        {
            //判断是否到达最大距离且未停止移动
            if (Vector2.Distance(throwPosition, transform.position) > maxTravelDistance && !wasStopped)
            {
                //停止移动
                StopWhenSpinning();

            }
            if (wasStopped)
            {
                //启动旋转计时器和伤害计时器
                spinTimer -= Time.deltaTime;
                hitTimer -= Time.deltaTime;

                //旋转时间到0时，停止旋转并返回
                if (spinTimer < 0)
                {
                    isReturn = true;
                    isSpinning = false;
                }
                //伤害时间为零时触发一段伤害，并重新计时
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            EnemyDamage(hit.GetComponent<Enemy>());
                    }
                }

            }
        }
    }

    /// <summary>
    /// 弹射特性逻辑
    /// </summary>
    private void Bouncing()
    {
        //判断是否处于可弹射状态，且可弹射敌人列表不为空
        if (isBouncing && enemyTarget.Count > 0)
        {
            //移动到敌人位置
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].transform.position, bouncingSpeed * Time.deltaTime);
            //判断是否达到敌人位置
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].transform.position) < .1f)
            {
                //触发敌人伤害函数
                EnemyDamage(enemyTarget[targetIndex].GetComponent<Enemy>(), bounceFreezeTime, true);
                //更换目标，弹射次数减少
                targetIndex++;
                amountOfBounce--;
                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
                //判断弹射次数是否为0或者是否可弹射敌人只有一个（避免一个敌人也可弹射多次）
                if (amountOfBounce <= 0 || enemyTarget.Count == 1)
                {
                    isBouncing = false;
                    isReturn = true;
                }
            }
        }
    }

    //初始化剑抛出的方向与重力
    public void SetUpSword(Vector2 _dir, float _gravity, Player _player, float _returnSpeed, float _freezeTime)
    {
        //设置剑的方向与重力
        rb.velocity = _dir;
        rb.gravityScale = _gravity;
        player = _player;
        returnSpeed = _returnSpeed;
        freezeTime = _freezeTime;
    }

    //初始化弹射特性
    public void SetUpBounce(int _amountOfBounce, float _bouncingSpeed, bool _isBouncing, float _bounceFreezeTime)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
        enemyTarget = new List<GameObject>();
        bouncingSpeed = _bouncingSpeed;
        bounceFreezeTime = _bounceFreezeTime;
    }

    //初始化穿刺特性
    public void SetUpPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    //初始化旋转特性
    public void SetUpSpin(Vector2 _throwPosition, float _maxTravelDistance, float _spinDuration, float _hitCooldown, bool _isSpinning)
    {
        maxTravelDistance = _maxTravelDistance;
        isSpinning = _isSpinning;
        spinDuration = _spinDuration;
        throwPosition = _throwPosition;
        hitCooldown = _hitCooldown;
    }

    /// <summary>
    /// 旋转特性技能移动停止
    /// </summary>
    public void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
        hitTimer = hitCooldown;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.GetComponent<Enemy>() != null)
        {
            EnemyDamage(collider.GetComponent<Enemy>(), freezeTime, true);
        }

        if (isReturn)
        {
            return;
        }

        SetupTargetForBounce(collider);

        StackInto(collider);


    }

    private void SetupTargetForBounce(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() != null)
        {

            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.gameObject);
                }
            }
        }
    }

    public void StackInto(Collider2D collider)
    {

        if (isSpinning)
        {
            if (!wasStopped)
                StopWhenSpinning();
            return;
        }
        if (pierceAmount > 0 && collider.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        canRotate = false;
        cd.enabled = false;
        if (isBouncing && enemyTarget.Count > 0)
            return;
        anim.SetBool("Flip", canRotate);
        transform.parent = collider.transform;
    }

    public void ReturnSword()
    {

        isReturn = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        transform.parent = null;

    }

    /// <summary>
    /// 对敌人造成伤害
    /// </summary>
    /// <param name="enemy">敌人对象</param>
    /// <param name="_seconds">冻结时间</param>
    /// <param name="_isFreeze">是否冻结</param>
    public void EnemyDamage(Enemy enemy, float _seconds = 0, bool _isFreeze = false)
    {

        player.stat.DoDamage(enemy.stat);
        if (_isFreeze)
            enemy.StartCoroutine("FreezeTimerFor", _seconds);
    }
}
