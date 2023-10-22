using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCapturingState : EnemyBaseState
{
    PolygonCollider2D baseCollider;
    Vector2 currentTarget;
    bool bReachedEndOfPath = false;
    public EnemyCapturingState(EnemyStateManager.EnemyState state, EnemyPathfinding pathfinding, PolygonCollider2D baseCollider) : base(state, pathfinding)
    { 
        this.baseCollider = baseCollider;
    }

    public override void EnterState()
    {
        Debug.Log("Entered Capturing State");
        EnemyPathfinding.ReachedEndOfPath += OnEndOfPathReached;

        if (baseCollider == null)
            return;

        Vector2 point = GetRandomPointInCollider(baseCollider);
        while (!(Physics2D.OverlapPoint(point, 1<<7) == baseCollider))
        {
            point = GetRandomPointInCollider(baseCollider);
        }

        currentTarget = point;
        pathfinding.CalculateNewPath(currentTarget);
    }

    public override void ExitState()
    {
        EnemyPathfinding.ReachedEndOfPath -= OnEndOfPathReached;
    }

    public override EnemyStateManager.EnemyState GetNextState()
    {
        if (!bReachedEndOfPath)
            return stateKey;
        else
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

    private void OnEndOfPathReached()
    {
        Debug.Log("End of Path reached");
        bReachedEndOfPath = true;
    }
}
