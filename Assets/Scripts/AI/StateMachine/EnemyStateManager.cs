using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : StateManager<EnemyStateManager.EnemyState>
{
    [SerializeField]
    private EnemyPathfinding pathfinding;
    [SerializeField]
    EnemyPerception enemyPerception;

    public PolygonCollider2D baseCollider = null;

    private void Start()
    {
        baseCollider = GameObject.FindWithTag("Base").GetComponent<PolygonCollider2D>();

        currentState = new EnemyCapturingState(EnemyState.Capturing, this, pathfinding, baseCollider);
        states.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle, this, pathfinding));
        states.Add(EnemyState.Shooting, new EnemyShootingState(EnemyState.Shooting, this, pathfinding));
        states.Add(EnemyState.Reloading, new EnemyReloadingState(EnemyState.Reloading, this, pathfinding));

        currentState.EnterState();
    }

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
        
    }

}
