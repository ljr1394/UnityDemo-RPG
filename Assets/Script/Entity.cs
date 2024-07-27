using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region 组件
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public EntityFX fx { get; private set; }
    public CharacterStats stat { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    private bool isKnockback;
    [Header("Move Info")]
    public float moveSpeed;
    public float defaultMoveSpeed { get; private set; }

    [Header("Collsion info")]
    [SerializeField] public Transform attackCheck;
    [SerializeField] public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;



    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;
    private Color defaultColor;

    public System.Action onFlipped;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        sr = GetComponentInChildren<SpriteRenderer>();
        stat = GetComponent<CharacterStats>();
        defaultColor = sr.color;
        defaultMoveSpeed = moveSpeed;
    }
    protected virtual void Update()
    {

    }
    /// <summary>
    /// 对象击退
    /// </summary>
    /// <returns></returns>
    public IEnumerator HitKnockback()
    {
        isKnockback = true;
        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;
    }

    public virtual void SlowEntityBy(float _slowPercentage, float _duration)
    {
        moveSpeed *= 1 - _slowPercentage;
        anim.speed = 1 - _slowPercentage;
        Invoke("ReturnDefaultSpeed", _duration);
    }
    public virtual void ReturnDefaultSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        anim.speed = 1;
    }

    #region 刚体
    /// <summary>
    /// 设置对象刚体速度
    /// </summary>
    /// <param name="_xVelocity">x速度</param>
    /// <param name="_yVelocity">y速度</param>
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnockback)
            return;
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FilpController(_xVelocity);
    }
    /// <summary>
    /// 设置对象刚体速度为0
    /// </summary>
    public void ZeroVelocity()
    {
        if (isKnockback)
            return;
        rb.velocity = Vector2.zero;
    }
    #endregion

    #region 碰撞
    /// <summary>
    /// 对象伤害函数
    /// </summary>
    public virtual void DamageKnockback() => StartCoroutine("HitKnockback");




    /// <summary>
    /// 检测对象是否站在地面
    /// </summary>
    /// <returns></returns>
    public bool IsGroundDetected() =>
    //Physics2D.Raycast（发射位置，方向，长度，物体层级） 发出射线检测射线范围内是否有指定层级物体
    Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    /// <summary>
    /// 检测对象是否靠墙
    /// </summary>
    /// <returns></returns>
    public bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    /// <summary>
    /// 射线检测
    /// </summary>
    protected void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion
    #region 翻转
    /// <summary>
    /// 翻转对象方向
    /// </summary>
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        if (onFlipped != null)
            onFlipped();
    }
    /// <summary>
    /// 判断对象是否需要进行翻转并进行翻转动作
    /// </summary>
    /// <param name="_x"></param>
    public virtual void FilpController(float _x)
    {
        if (_x > 0 && !facingRight || _x < 0 && facingRight)
        {
            Flip();
        }

    }

    public void SetVisible(bool _isVisible)
    {

        if (_isVisible)
            sr.color = defaultColor;
        else
            sr.color = Color.clear;
    }
    #endregion

    public virtual void Die()
    {

    }


}
