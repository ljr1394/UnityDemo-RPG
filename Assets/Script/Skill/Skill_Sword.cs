
using UnityEngine;

public enum SwordType
{
    Reguler,
    Bounce,
    Pierce,
    Spin
}

public class Skill_Sword : Skill
{
    [SerializeField] private SwordType swordType;
    [Header("Bounce Info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bouncingSpeed;
    [SerializeField] private float bounceFrezzTime;

    [Header("Pierce Info")]
    [SerializeField] private int pierceamount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;
    [SerializeField] private float hitCooldown = .35f;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float freezeTime;


    private Vector2 finalDir;

    [Header("AimDot Info")]
    [SerializeField] protected int dotsNumber;
    [SerializeField] protected float dotSpace;
    [SerializeField] protected GameObject dotPrefab;
    [SerializeField] protected Transform dotsParent;
    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        SetGravity();
        GenerateDot();
    }
    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            //获取玩家位置到鼠标位置的单位向量
            Vector2 aimDirection = AimDirection().normalized;
            //声明抛出剑的力与方向
            finalDir = new Vector2(aimDirection.x * launchForce.x, aimDirection.y * launchForce.y);
        }
        if (Input.GetKey(KeyCode.E))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotPosition(i * dotSpace);
            }

        }
    }

    public void SetGravity()
    {
        switch (swordType)
        {
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Reguler:
                swordGravity = 4.5f;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
        }
    }
    /// <summary>
    /// 创建剑对象
    /// </summary>
    public void CreateSword()
    {
        Debug.Log(player == null);
        //创建剑对象
        GameObject sword = Instantiate(swordPrefab, player.transform.position, player.transform.rotation);
        Sword_Skill_Controller swordController = sword.GetComponent<Sword_Skill_Controller>();
        switch (swordType)
        {
            case SwordType.Bounce:
                swordController.SetUpBounce(bounceAmount, bouncingSpeed, true, bounceFrezzTime);
                break;
            case SwordType.Pierce:
                swordController.SetUpPierce(pierceamount);
                break;
            case SwordType.Reguler:

                break;
            case SwordType.Spin:
                swordController.SetUpSpin(player.transform.position, maxTravelDistance, spinDuration, hitCooldown, true);
                break;
        }
        //初始化剑对象
        swordController.SetUpSword(finalDir, swordGravity, player, returnSpeed, freezeTime);
        player.AssginNewSword(sword);
        //禁用瞄准dots
        DotsActive(false);
    }

    #region 瞄准线
    /// <summary>
    /// 设置所有Dot的激活状态
    /// </summary>
    /// <param name="_isActive"></param>
    public void DotsActive(bool _isActive)
    {
        foreach (var dot in dots)
        {
            dot.SetActive(_isActive);
        }
    }
    /// <summary>
    /// 玩家到鼠标位置的向量
    /// </summary>
    /// <returns></returns>
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }
    /// <summary>
    /// 创建dot对象
    /// </summary>
    private void GenerateDot()
    {
        dots = new GameObject[dotsNumber];

        for (int i = 0; i < dotsNumber; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }

    }

    /// <summary>
    /// 设置某个时刻dot的位置
    /// </summary>
    /// <param name="t">时间</param>
    /// <returns></returns>
    private Vector2 DotPosition(float t)
    {
        //武器投掷的单位向量
        Vector2 playerPosition = player.transform.position;
        Vector2 aimDir = AimDirection().normalized;
        //抛物线公式 物体在t时刻的位置 = v0 * t + 1/2*g*t^2
        return playerPosition + new Vector2(aimDir.x * launchForce.x, aimDir.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);
    }
    #endregion
}
