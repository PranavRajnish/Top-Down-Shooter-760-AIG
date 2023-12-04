using Player;
using UnityEngine;
using Weapons;

namespace AI.StateMachine.EnemyStates
{
    public class EnemyReloadingState : EnemyBaseState
    {
        private Gun _currentGun;
        private readonly CharacterDefenseStats _defenseStats;
        private Gun CurrentGun => Enemy.CurrentGun;

        public EnemyReloadingState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager) 
        {
            _defenseStats = stateManager.gameObject.GetComponent<CharacterDefenseStats>();
        }

        private bool _hideBeforeReload;
        private bool _reloadAttempted;

        public override void EnterState()
        {
            _hideBeforeReload = false;
            _reloadAttempted = false;
            if (CurrentGun is ARGun)
            {
                // If health is low hide before reloading, otherwise reload without hiding.
            
                if(_defenseStats.NormalizedHealth <= 0.4f)
                {
                    Debug.Log("AR Gun low health");
                    _hideBeforeReload = true;
                }
                else
                {
                    Debug.Log("AR Gun high health");
                    CurrentGun.OnReloadPressed();
                    _reloadAttempted = true;
                }
            }
            else
            {
                _hideBeforeReload = true;
            }

            // Checking to see if enemy has already attempted to hide.
            if(stateManager.previousState == EnemyStateManager.EnemyState.Hiding)
            {
                _hideBeforeReload = false;
                if (!_reloadAttempted)
                    CurrentGun.OnReloadPressed();
            }

        }

        public override EnemyStateManager.EnemyState GetNextState()
        {
            if(_hideBeforeReload)
            {
                return EnemyStateManager.EnemyState.Hiding;
            }

            if (CurrentGun.BulletsRemaining <= 0)
                return StateKey;

            if (Perception.CanSeePlayer)
                return EnemyStateManager.EnemyState.Shooting;

            return EnemyStateManager.EnemyState.FindPlayer;
        }
    }
}
