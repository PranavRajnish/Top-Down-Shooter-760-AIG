using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : StateManager<EnemyStateManager.EnemyState>
{
    public enum EnemyState
    {
        Capturing,
        Idle,
        Shooting,
        Reloading,
    };

    // Start is called before the first frame update
    private void Awake()
    {
        currentState = new EnemyCapturingState(EnemyState.Capturing);
        states.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle));
        states.Add(EnemyState.Shooting, new EnemyIdleState(EnemyState.Shooting));
        states.Add(EnemyState.Reloading, new EnemyIdleState(EnemyState.Reloading));
    }

}
