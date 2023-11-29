using System.Collections;
using System.Collections.Generic;
using AI.StateMachine.EnemyStates;
using UnityEngine;
using Weapons;

public class EnemyHidingState : EnemyBaseState
{
    public EnemyHidingState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager) { }

    private bool bHidingAttemptFinished = false;
    private bool bTransitionToFindPlayerState = false;
    private float hidingTime;


    public override void EnterState()
    {
        Debug.Log("Entered Hiding State");

        Pathfinding.hidingAttemptFinished += OnHidingAttemptFinished;
        Pathfinding.FindCover(Perception.player.transform);

        hidingTime = Random.Range(stateManager.sniperWaitingTimeMin, stateManager.sniperWaitingTimeMax);
    }

    public override void ExitState()
    {
        Pathfinding.hidingAttemptFinished -= OnHidingAttemptFinished;
    }

    public override EnemyStateManager.EnemyState GetNextState()
    {     
        if(bHidingAttemptFinished)
        {
            return EnemyStateManager.EnemyState.Reloading;
        }

        return StateKey;

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
        Debug.Log("Hiding Attempt Finished");
        bHidingAttemptFinished = true;
    }
}