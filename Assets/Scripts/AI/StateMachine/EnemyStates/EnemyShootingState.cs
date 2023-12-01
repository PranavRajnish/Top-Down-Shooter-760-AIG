using Player;
using UnityEngine;
using Weapons;

namespace AI.StateMachine.EnemyStates
{
    public class EnemyShootingState : EnemyBaseState
    {
        private Transform Transform => stateManager.transform;

        private readonly CharacterDefenseStats _defenseStats;
        private Gun CurrentGun => Enemy.CurrentGun;

        public EnemyShootingState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager)
        {
            _defenseStats = stateManager.gameObject.GetComponent<CharacterDefenseStats>();
            // _playerRigidbody = Perception.player.GetComponent<Rigidbody>();
        }

        public override void ExitState()
        {
            base.ExitState();

            if (CurrentGun)
                CurrentGun.OnTriggerReleased();
        }

        public override EnemyStateManager.EnemyState GetNextState()
        {
            if (CurrentGun.BulletsRemaining <= 0)
                return EnemyStateManager.EnemyState.Reloading;
            
            if (_defenseStats.NormalizedHealth <= 0.3f)
                return EnemyStateManager.EnemyState.Hiding;

            if (Perception.CanSeePlayer)
                return StateKey;

            return EnemyStateManager.EnemyState.FindPlayer;
        }

        public override void UpdateState()
        {
            if (Perception.CanSeePlayer)
            {
                var direction = Perception.player.transform.position - CurrentGun.Muzzle.position;
                Transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));

                CurrentGun.OnTriggerPulled();
            }

            if (CurrentGun.CurrentFireMode != FireMode.Auto)
                CurrentGun.OnTriggerReleased();
        }

        public override void FixedUpdateState()
        {
            if (!Perception.CanSeePlayer) return;
            if (Pathfinding.IsStrafing) return;

            switch (CurrentGun)
            {
                case ARGun:
                case Pistol:
                    Pathfinding.Strafe(Vector2.Perpendicular((Perception.player.transform.position - Transform.position).normalized));
                    break;
            }
        }
    }
}