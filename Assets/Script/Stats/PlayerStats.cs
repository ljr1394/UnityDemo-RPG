using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void DoDamage(CharacterStats _stats)
    {
        base.DoDamage(_stats);
    }
    public override void TakeDamage(int _damage)
    {
        player.DamageKnockback();
        base.TakeDamage(_damage);

    }
    public override void Die()
    {
        base.Die();
        player.Die();
    }
}
