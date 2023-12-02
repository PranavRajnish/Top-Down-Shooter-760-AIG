using AI.StateMachine.EnemyStates;
using UnityEngine;

public class EnemyFindPlayerState : EnemyBaseState
{
    private PolygonCollider2D baseCollider => stateManager.BaseCollider;
    private Vector2 _currentTarget;
    private double _startTime;
    private bool _hadSeenPlayer;
    
    public EnemyFindPlayerState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered Find Player State");

        stateManager.gotHit = false;

        _currentTarget = Perception.player.transform.position;
        Pathfinding.CalculateNewPath(_currentTarget);
        _startTime = Time.time;
    }

    public override void ExitState()
    {
        base.ExitState();

        Pathfinding.StopCalculatingPath();
    }

    public override EnemyStateManager.EnemyState GetNextState()
    {
        if (Perception.CanSeePlayer)
            return EnemyStateManager.EnemyState.Shooting;
        
        if (Time.time - _startTime > 5)
            return EnemyStateManager.EnemyState.Capturing;

        return StateKey;
    }

    public override void UpdateState()
    {
        if (!Perception.player.transform.position.Equals(_currentTarget))
        {
            _currentTarget = Perception.player.transform.position;
            Pathfinding.CalculateNewPath(_currentTarget);
        }
    }
}