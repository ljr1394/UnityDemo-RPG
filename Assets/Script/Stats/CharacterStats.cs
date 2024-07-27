using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    private EntityFX fx;
    private Entity entity;
    private Animator anim;

    [Header("Mojor stats")]
    public Stat strenght;
    public Stat agility;
    public Stat intelligencs;
    public Stat vitality;

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critPower;
    public Stat critChange;



    [Header("Defensive stats")]
    public Stat armor;
    public Stat maxHealth;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat iceDamage;
    public Stat fireDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChill;
    public bool isShock;

    private int igniteDamage = 2;
    public int shockDamage;

    #region 定时器
    private float chillTimer;
    private float ignitedTimer;
    private float shockTimer;
    private float igniteDamageTimer;
    private float igniteDamageCooldown = .5f;
    public float alimentsDuration = 5;
    #endregion

    [SerializeField] private GameObject shockStrikePrefab;
    public System.Action onHealthChanged;
    public int currentHealth;
    protected virtual void Start()
    {
        currentHealth = GetMaxHealthValue();
        critPower.SetValue(150);
        fx = GetComponent<EntityFX>();
        entity = GetComponent<Entity>();
        anim = GetComponentInChildren<Animator>();

    }

    protected virtual void Update()
    {
        chillTimer -= Time.deltaTime;
        ignitedTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }
        if (shockTimer < 0)
        {
            isShock = false;
        }
        if (chillTimer < 0)
        {
            isChill = false;

        }

        if (igniteDamageTimer < 0 && isIgnited)
        {
            ChangeHealth(igniteDamage);
            if (currentHealth <= 0)
                Die();
            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasin = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (Random.Range(0, 100) < totalEvasin)
        {
            return true;
        }

        return false;
    }



    public virtual void DoDamage(CharacterStats _targetStats)
    {

        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strenght.GetValue();
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        //DoMagicDamage(_targetStats);
        _targetStats.TakeDamage(totalDamage);
    }


    public void SetIgnitedDamage(int _igniteDamage) => igniteDamage = _igniteDamage;
    public void SetShockDamage(int _shockDamage)
    {
        shockDamage = _shockDamage;

    }

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _iceDamage = iceDamage.GetValue();
        int _fireDamage = fireDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        int totalMagicDamage = _iceDamage + _fireDamage + _lightningDamage + intelligencs.GetValue();
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);

        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_iceDamage, _fireDamage, _lightningDamage) <= 0)
            return;

        bool canChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canShock = _lightningDamage > _iceDamage && _lightningDamage > _fireDamage;
        bool canIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;

        if (!canChill && !canIgnite && !canShock)
        {
            while (true)
            {
                if (Random.value < 0.3f)
                {
                    canChill = true;
                    break;
                }
                else if (Random.value < 0.6f)
                {
                    canShock = true;
                    break;
                }
                else
                {
                    canIgnite = true;
                    break;
                }
            }
        }
        if (canIgnite)
            _targetStats.SetIgnitedDamage(Mathf.RoundToInt(_fireDamage * .1f));

        if (canShock)
            _targetStats.SetShockDamage(Mathf.RoundToInt(_lightningDamage * .2f));

        _targetStats.ApplyAilments(canChill, canShock, canIgnite);
    }

    public int CalculateCriticalDamage(int _damage)
    {
        float totalCriticalDamage = (critPower.GetValue() + strenght.GetValue()) * 0.01f;

        float critDamage = totalCriticalDamage * _damage;
        return Mathf.RoundToInt(critDamage);
    }

    public void ApplyAilments(bool _isChill, bool _isShock, bool _isIgnited)
    {
        Debug.Log(shockDamage);
        bool canApplyIgnited = !_isChill && !_isShock && !_isIgnited;
        bool canApplyChill = !_isChill && !_isShock && !_isIgnited;
        bool canApplyShock = !_isChill && !_isIgnited;

        if (_isChill && canApplyChill)
        {
            isChill = _isChill;
            chillTimer = alimentsDuration;
            entity.SlowEntityBy(.4f, chillTimer);
            fx.ChillFXFor(chillTimer);
        }

        if (_isIgnited && canApplyIgnited)
        {
            isIgnited = _isIgnited;
            ignitedTimer = alimentsDuration;
            fx.IgniteFXFor(ignitedTimer, igniteDamageCooldown / 2);
        }

        if (_isShock && canApplyShock)
        {
            if (!isShock)
            {
                ApplyShock(_isShock);
            }
            else if (GetComponent<Player>() == null)
            {

                StrikeCloseEnemy();

            }


        }
    }

    public void ApplyShock(bool _isShock)
    {
        if (isShock)
        {
            return;
        }
        isShock = _isShock;
        shockTimer = alimentsDuration;
        fx.ShockFXFor(shockTimer, .5f);
    }

    public void StrikeCloseEnemy()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);
        float closeDiration = int.MaxValue;
        Transform closeEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null
                && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closeDiration)
                {
                    closeDiration = distance;
                    closeEnemy = hit.transform;
                }
            }

        }
        if (closeEnemy != null)
        {
            CharacterStats _targetStats = closeEnemy.GetComponent<CharacterStats>();
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ThunderStrike_Controller>().Setup(shockDamage, _targetStats);
        }

    }
    public int CheckTargetArmor(CharacterStats _targetStats, int _damage)
    {
        int targetDamage;
        if (isChill)
            targetDamage = Mathf.RoundToInt(_damage - (_targetStats.armor.GetValue() * (1 - .2f)));
        else
            targetDamage = _damage - _targetStats.armor.GetValue();
        return Mathf.Clamp(targetDamage, 1, int.MaxValue);
    }

    public int CheckTargetResistance(CharacterStats _targetStats, int _magicDamage)
    {
        _magicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligencs.GetValue() * 3);
        return Mathf.Clamp(_magicDamage, 1, int.MaxValue);
    }

    public bool CanCrit()
    {
        int totalCrit = critChange.GetValue() + agility.GetValue();
        if (isShock)
            totalCrit += 20;
        if (Random.Range(0, 100) < totalCrit)
        {
            return true;
        }
        return false;
    }
    public virtual void TakeDamage(int _damage)
    {
        ChangeHealth(_damage);
        fx.StartCoroutine("FlashFX");
        entity.DamageKnockback();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ChangeHealth(int _damage)
    {
        currentHealth -= _damage;
        onHealthChanged();
    }

    public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;

    public virtual void Die()
    {

    }
}
