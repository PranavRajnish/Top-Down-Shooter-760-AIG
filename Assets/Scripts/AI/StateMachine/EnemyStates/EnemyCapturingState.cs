using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCapturingState : EnemyBaseState
{
    private PolygonCollider2D baseCollider => stateManager.BaseCollider;
    Vector2 currentTarget;

    public EnemyCapturingState(EnemyStateManager.EnemyState state, EnemyStateManager stateManager) : base(state, stateManager)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered Capturing State");

        if (baseCollider == null)
            return;


        Vector2 point = GetRandomPointInCollider(baseCollider);
        while (!(Physics2D.OverlapPoint(point, 1 << 7) == baseCollider))
        {
            point = GetRandomPointInCollider(baseCollider);
        }

        currentTarget = point;
        Pathfinding.CalculateNewPath(currentTarget);
    }

    public override void ExitState()
    {
        Pathfinding.StopCalculatingPath();
    }

    public override EnemyStateManager.EnemyState GetNextState()
    {
        if (Perception.CanSeePlayer)
            return EnemyStateManager.EnemyState.Shooting;

        if (!Pathfinding.ReachedEndOfPath)
            return stateKey;
        
        return EnemyStateManager.EnemyState.Idle;
    }

    public override void OnTriggerEnter(Collider2D other)
    {
    }

    public override void OnTriggerExit(Collider2D other)
    {
    }

    public override void OnTriggerStay(Collider2D other)
    {
    }

    public override void UpdateState()
    {
    }

    Vector2 GetRandomPointInCollider(PolygonCollider2D collider)
    {
        var point = new Vector2(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y)
        );

        return point;
    }
}