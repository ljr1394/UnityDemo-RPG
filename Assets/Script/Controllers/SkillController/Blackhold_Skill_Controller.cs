using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhold_Skill_Controller : MonoBehaviour
{

    [Header("hotKey Info")]
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodes;
    [SerializeField] private float hotKeyDuration;


    #region  基础属性
    private float maxSize;
    private float growSpeed;
    private float blackholdDuration;
    private float shrinkSpeed;
    private float shrinkDelayed = 1f;
    private float shrinkTimer;
    #endregion

    #region 克隆属性
    private float amountOfAttacks;
    private float cloneCooldown;
    #endregion

    #region 状态属性
    private bool isShrink;
    private bool canGrow = true;
    public bool isComplish { get; private set; }
    #endregion

    private HotKey_Controller hasHotKey;
    private List<Transform> targets = new List<Transform>();

    void Update()
    {

        blackholdDuration -= Time.deltaTime;

        //判断黑洞是否可扩张
        if (canGrow)
        {
            //黑洞扩张
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        //判断黑洞是否具备收缩条件
        if (blackholdDuration < 0 && !isShrink)
        {
            //设置黑洞收缩状态
            canGrow = false;
            isShrink = true;
            shrinkTimer = shrinkDelayed;
        }

        //判断黑洞是否进入收缩状态
        if (isShrink)
        {
            shrinkTimer -= Time.deltaTime;
            BlackholdShrink();
        }


        //判断是否生成热键
        if (targets.Count > 0 && !hasHotKey && !isShrink)
        {
            int targetIndex = Random.Range(0, targets.Count);
            CreateHotKey(targets[targetIndex]);
        }

        //监听热键是否按下
        if (hasHotKey)
            ListenHotKeyDown();
    }
    /// <summary>
    /// 设置黑洞属性
    /// </summary>
    /// <param name="_position"></param>
    /// <param name="_maxSize"></param>
    /// <param name="_growSpeed"></param>
    /// <param name="_blackholdDuration"></param>
    /// <param name="_amountOfAttacks"></param>
    /// <param name="_cloneCooldown"></param>
    /// <param name="_shrinkSpeed"></param>
    public void SetupBlackhold(Vector2 _position, float _maxSize, float _growSpeed, float _blackholdDuration, float _amountOfAttacks, float _cloneCooldown, float _shrinkSpeed)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        blackholdDuration = _blackholdDuration;
        transform.position = _position;
        amountOfAttacks = _amountOfAttacks;
        cloneCooldown = _cloneCooldown;
        shrinkSpeed = _shrinkSpeed;

    }
    /// <summary>
    /// 黑洞收缩
    /// </summary>
    public void BlackholdShrink()
    {
        //销毁还存在的热键
        if (hasHotKey)
            Destroy(hasHotKey.gameObject);
        if (shrinkTimer < 0)
            //收缩
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
        //是否收缩完成
        if (transform.localScale.x < 0)
        {
            //销毁黑洞
            isComplish = true;
        }
    }
    /// <summary>
    /// 监听热键，执行动作
    /// </summary>
    public void ListenHotKeyDown()
    {
        if (hasHotKey.IsKeyDown())
        {
            //设置热键为消失状态
            hasHotKey.SetLoosing(true);
            //执行克隆攻击
            StartCoroutine("cloneAttack", hasHotKey.GetBind().transform);
            //置空当前热键
            hasHotKey = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            //冻结敌方单位
            enemy.FreezeTime(true);
            //敌方单位被攻击后不会移动
            enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            //添加敌人进入敌人列表
            AddEnemyToList(enemy.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            //解冻地方单位
            enemy.FreezeTime(false);
            enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    /// <summary>
    /// 创建热键
    /// </summary>
    /// <param name="_enemy"></param>
    private void CreateHotKey(Transform _enemy)
    {
        Vector2 hotKeyPosition = new Vector2(_enemy.position.x, _enemy.position.y + 2);

        //实例化热键
        GameObject hotKeyObj = Instantiate(hotKeyPrefab);

        //随机生成热键码
        KeyCode chooseHotKey = keyCodes[Random.Range(0, keyCodes.Count)];
        HotKey_Controller hotKeyScript = hotKeyObj.GetComponent<HotKey_Controller>();

        //黑洞当前持有一个热键
        hasHotKey = hotKeyScript;

        //初始化热键
        hotKeyScript.SetupHotkey(chooseHotKey, hotKeyPosition, hotKeyDuration, 5f);
        hasHotKey.SetBind(_enemy.gameObject);
    }

    /// <summary>
    /// 克隆攻击
    /// </summary>
    /// <param name="_targetTransform"></param>
    /// <returns></returns>
    private IEnumerator cloneAttack(Transform _targetTransform)
    {
        if (!SkillManager.instance.skill_Clone.crystalInsteadOfClone)
        {
            PlayerManager.instance.player.SetVisible(false);
        }
        //根据设置的攻击次数，依次攻击
        for (int i = 0; i < amountOfAttacks; i++)
        {
            //生成克隆体坐标偏移量
            Vector2 offset = new Vector2(i % 2 == 0 ? 2 : -2, 0);
            //创建克隆体并进行攻击
            SkillManager.instance.skill_Clone.CreateClone(_targetTransform, true, offset);
            if (SkillManager.instance.skill_Clone.crystalInsteadOfClone)
            {
                float radius = GetComponent<CircleCollider2D>().radius * maxSize;
                SkillManager.instance.skill_Crystal.currentCrystal.GetComponent<Crystal_Skill_Controller>().RandomChooseTarget(radius);
            }
            //攻击间隔
            yield return new WaitForSeconds(cloneCooldown);
        }

    }
    public void AddEnemyToList(Transform enemy)
    {
        targets.Add(enemy);
    }
}
