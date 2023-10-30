using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class EnemyReloadingState : EnemyBaseState
{
    private Gun _currentGun;
    public EnemyReloadingState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager) { }

    public override void EnterState()
    {
        _currentGun = stateManager.gameObject.GetComponent<Enemy>().currentGun;
        _currentGun.OnReloadPressed();
    }

    public override void ExitState()
    {
    }

    public override EnemyStateManager.EnemyState GetNextState()
    {
        if (_currentGun.BulletsRemaining <= 0)
            return stateKey;

        if (Perception.CanSeePlayer)
            return EnemyStateManager.EnemyState.Shooting;

        return EnemyStateManager.EnemyState.FindPlayer;
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
}
