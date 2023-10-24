using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    
    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> currentState;

    protected bool isTransitioningState = false;
    void Start()
    {
        currentState.EnterState();
    }

    void Update()
    {
        
        EState nextStateKey = currentState.GetNextState();
        if(!isTransitioningState && nextStateKey.Equals(currentState.stateKey))
        {
            currentState.UpdateState();
        }
        else if (!isTransitioningState)
        {
            TransitionToState(nextStateKey);
        }

        currentState.UpdateState();
        
    }

    protected void TransitionToState(EState nextStateKey)
    {
        isTransitioningState = true;
        currentState.ExitState();
        currentState = states[nextStateKey];
        currentState.EnterState();
        isTransitioningState = false;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter(collision);
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        currentState.OnTriggerStay(collision);
    }

    protected void OnTrigerExit(Collider2D collision) 
    {
        currentState.OnTriggerExit(collision);
    }
}
