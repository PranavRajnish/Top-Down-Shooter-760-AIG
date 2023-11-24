using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class EnemyHidingState : EnemyBaseState
{
    public EnemyHidingState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager) { }

    public override void EnterState()
    {
        Pathfinding.hidingAttemptFinished += OnHidingAttemptFinished;
        Pathfinding.FindCover(Perception.player.transform);
    }

    public override void ExitState()
    {
        Pathfinding.hidingAttemptFinished -= OnHidingAttemptFinished;
    }

    public override EnemyStateManager.EnemyState GetNextState()
    {     
        return stateKey;

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

    private void OnHidingAttemptFinished()
    {

    }
}