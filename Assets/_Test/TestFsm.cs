﻿using System.Threading;
using KKSFramework.Fsm;
using UniRx.Async;
using UnityEngine;

public enum State
{
    Idle,
    Attack,
    Defence
}


public class TestFsm : MonoBehaviour
{
    private FsmRunner _fsmRunner;

    public State state;

    public bool isStop;
    
    private void Start ()
    {
        _fsmRunner = new FsmRunner ();
        _fsmRunner.RegistFsmState (State.Attack.ToString(), AttackState);
        _fsmRunner.RegistFsmState (State.Defence.ToString(), DefenceState);
        _fsmRunner.RegistFsmState (State.Idle.ToString(), IdleState);
        _fsmRunner.StartFsm (state.ToString());
    }

    private void Update ()
    {
        if (isStop)
        {
            isStop = false;
            StopFsm ();
        }
    }


    private void StopFsm ()
    {
        _fsmRunner.StopFsm ();
    }
    
    
    private async UniTask AttackState (CancellationTokenSource cts)
    {
        state = State.Attack;
        Debug.Log ("attack state");
        await UniTask.Delay (1000, cancellationToken:cts.Token);
    }
    
    private async UniTask DefenceState (CancellationTokenSource cts)
    {
        state = State.Defence;
        Debug.Log ("def state");
        await UniTask.Delay (1000, cancellationToken:cts.Token);
    }
    
    private async UniTask IdleState (CancellationTokenSource cts)
    {
        state = State.Idle;
        Debug.Log ("idle state");
        await UniTask.Delay (1000, cancellationToken:cts.Token);
    }
}
