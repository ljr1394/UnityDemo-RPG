using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAnimotionTriggers : MonoBehaviour
{
    protected Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    //动画结束触发器
    public void AnimotionTrigger()
    {
        enemy.AnimotionFinishTrigger();
    }
    //攻击触发器
    public void AttackTrigger()
    {
        //获取攻击触发区域内所有的对象
        //Physics2D.OverlapCircleAll(触发区域圆心, 半径) 绘制一个圆形区域并获取区域内所有对象
        Collider2D[] collider = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        //遍历获取到的对象
        foreach (var hit in collider)
        {
            //如果攻击触发区域存在玩家对象
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats player = hit.GetComponent<PlayerStats>();
                enemy.GetComponent<EnemyStats>().DoDamage(player);
            }

        }
    }

    //打开可格挡状态
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    //关闭可格挡状态
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
