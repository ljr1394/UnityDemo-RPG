using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    [SerializeField] private int level;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier;
    protected override void Start()
    {
        Modifier(damage);
        Modifier(maxHealth);

        base.Start();
        enemy = GetComponent<Enemy>();




    }

    public void Modifier(Stat _stat)
    {
        int baseValue = _stat.GetValue();
        for (int i = 1; i < level; i++)
        {
            _stat.AddModifier(Mathf.RoundToInt(baseValue * percentageModifier));
        }

    }

    public override void DoDamage(CharacterStats _stats)
    {
        base.DoDamage(_stats);
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        enemy.DamageKnockback();
    }
    public override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
