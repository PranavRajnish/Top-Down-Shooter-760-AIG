using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : StateManager<EnemyStateManager.EnemyState>
{
    [SerializeField] private EnemyPathfinding pathfinding;
    [SerializeField] EnemyPerception enemyPerception;

    [SerializeField] private PolygonCollider2D baseCollider = null;

    public EnemyPathfinding Pathfinding => pathfinding;
    public EnemyPerception Perception => enemyPerception;
    public PolygonCollider2D BaseCollider => baseCollider;


    private void Start()
    {
        baseCollider = GameObject.FindWithTag("Base").GetComponent<PolygonCollider2D>();

        currentState = new EnemyCapturingState(EnemyState.Capturing, this);
        states.Add(EnemyState.Capturing, currentState);
        states.Add(EnemyState.FindPlayer, new EnemyFindPlayerState(EnemyState.FindPlayer, this));
        states.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle, this));
        states.Add(EnemyState.Shooting, new EnemyShootingState(EnemyState.Shooting, this));
        states.Add(EnemyState.Reloading, new EnemyReloadingState(EnemyState.Reloading, this));

        currentState.EnterState();
    }

    public enum EnemyState
    {
        Capturing,
        FindPlayer,
        Idle,
        Shooting,
        Reloading,
    };

    // Start is called before the first frame update
    private void Awake()
    {
    }
}