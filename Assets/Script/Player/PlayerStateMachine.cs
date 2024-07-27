using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    /// <summary>
    /// 初始化状态机
    /// </summary>
    /// <param name="_startState">角色当前状态</param>
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    /// <summary>
    /// 转换角色状态
    /// </summary>
    /// <param name="_newState">角色要切换的新状态</param>
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
