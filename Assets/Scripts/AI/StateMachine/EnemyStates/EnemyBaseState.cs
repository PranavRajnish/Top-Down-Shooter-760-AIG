using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  EnemyBaseState : BaseState<EnemyStateManager.EnemyState>
{
    protected EnemyPathfinding pathfinding;
    protected EnemyStateManager enemyStateManager;

    protected bool bPlayerInSight = false;
    public EnemyBaseState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager, EnemyPathfinding pathfinding) : base(state)
   {
        this.enemyStateManager = enemyStateManager;
        this.pathfinding = pathfinding;
   }

    public override void EnterState()
    {
        EnemyPerception.PlayerFound += OnPlayerFound;
        EnemyPerception.PlayerLost += OnPlayerLost;
    }

    public override void ExitState()
    {
        EnemyPerception.PlayerFound -= OnPlayerFound;
        EnemyPerception.PlayerLost -= OnPlayerLost;
    }

    protected virtual void OnPlayerFound()
    {
        bPlayerInSight = true;
    }

    protected virtual void OnPlayerLost()
    {
        bPlayerInSight = false;
    }


}
