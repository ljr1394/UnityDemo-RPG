using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimotionTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    public void AnimotionTrigger()
    {

        player.AnimotionTrigger();

    }

    public void ComboTrigger()
    {

        player.ComboTrigger();

    }

    public void AttackTrigger()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats enemy = hit.GetComponent<EnemyStats>();

                player.GetComponent<CharacterStats>().DoDamage(enemy);
            }



        }
    }

    public void ThrowTrigger()
    {
        SkillManager.instance.skill_Sword.CreateSword();
    }

}
