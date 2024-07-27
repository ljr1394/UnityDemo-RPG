using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimeryAttack : PlayerState
{
    private int comboCount;

    private float lastAttackTimer;
    private float comboWindow = 2;
    public PlayerPrimeryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //Á¬»÷¼ä¸ô
        /*if (comboCount > 2|| Time.time >lastAttackTimer +comboWindow)*/
        comboCount = 0;
        player.anim.SetInteger("ComboCounter", comboCount);
        player.SetVelocity(player.attackMovement[comboCount].x * player.facingDir, player.attackMovement[comboCount].y);
        stateTimer = .15f;

    }

    public override void Exit()
    {
        base.Exit();
       
       
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.J)&&comboTrigger) {
            comboTrigger = false;
            stateTimer = .15f;
            float attackDir=player.facingDir;
 
            if (inputX != 0) {
                attackDir = inputX;
            }
            player.SetVelocity(player.attackMovement[comboCount].x * attackDir, player.attackMovement[comboCount].y);
            
            comboCount++;
            if (comboCount > 2)
            {
                comboCount = 0;
            }
            player.anim.SetInteger("ComboCounter", comboCount);
            
            //lastAttackTimer = Time.time;
            return;
        }
           

        if (stateTimer < 0)
            player.ZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
            

       
    }

}
