using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using AI.StateMachine.EnemyStates;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private PolygonCollider2D baseCollider;
    private float timer;
    private bool bIsPatrolling = false;

    public EnemyIdleState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager) 
    {
        baseCollider = stateManager.BaseCollider;
        timer = stateManager.waitTimeBetweenPatrol;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");

        Pathfinding.reachedEndOfPath += OnReachedEndOfPath;
    }

    public override void ExitState()
    {
        base.ExitState();

        Pathfinding.reachedEndOfPath -= OnReachedEndOfPath;
    }

    public override EnemyStateManager.EnemyState GetNextState()
    {
        if (stateManager.gotHit)
            return EnemyStateManager.EnemyState.FindPlayer;
        
        if (Perception.CanSeePlayer)
            return EnemyStateManager.EnemyState.Shooting;
        
        return StateKey;
    }

    public override void UpdateState()
    {
        if(!bIsPatrolling)
        {
            timer -= Time.deltaTime;
        }
        if(timer <=0)
        {
            bIsPatrolling = true;
            FindNewPatrolPoint();
            timer = stateManager.waitTimeBetweenPatrol;
        }
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
    
    private void FindNewPatrolPoint()
    {
        float xPos = stateManager.transform.position.x;
        float yPos = stateManager.transform.position.y;
        float xRand = Random.Range(xPos - (stateManager.patrolPointRadius / 2), xPos + (stateManager.patrolPointRadius / 2));
        float yRand = Random.Range(yPos - (stateManager.patrolPointRadius / 2), yPos + (stateManager.patrolPointRadius / 2));
        Vector2 point = new Vector2(xRand, yRand);

        if(!(Physics2D.OverlapPoint(point, 1 << 7) == baseCollider))
        {
            FindNewPatrolPoint();
        }
        else
        {
            Pathfinding.CalculateNewPath(point);
        }
    }

    private void OnReachedEndOfPath()
    {
        bIsPatrolling = false;
    }
}
