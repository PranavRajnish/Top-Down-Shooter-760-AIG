using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : StateManager<EnemyStateManager.EnemyState>
{
    [SerializeField]
    private EnemyPathfinding pathfinding;
    [SerializeField]
    private PolygonCollider2D baseCollider;
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
        currentState = new EnemyCapturingState(EnemyState.Capturing, pathfinding, baseCollider);
        states.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle, pathfinding));
        states.Add(EnemyState.Shooting, new EnemyIdleState(EnemyState.Shooting, pathfinding));
        states.Add(EnemyState.Reloading, new EnemyIdleState(EnemyState.Reloading, pathfinding));
    }

}
