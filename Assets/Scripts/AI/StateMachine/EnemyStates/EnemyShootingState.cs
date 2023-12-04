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
        private float ChangeInHealth => _healthWhileEntering - _defenseStats.NormalizedHealth;

        private float _healthWhileEntering;

        private const float MinPistolDistance = 1f, MaxPistolDistance = 3.33f;
        private const float MinARDistance = 3f, MaxARDistance = 6.33f;
        private const float MinSniperDistance = 5f, MaxSniperDistance = 7.33f;

        public EnemyShootingState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager)
        {
            _defenseStats = stateManager.gameObject.GetComponent<CharacterDefenseStats>();
        }

        public override void EnterState()
        {
            base.EnterState();

            _healthWhileEntering = _defenseStats.NormalizedHealth;
        }

        public override void ExitState()
        {
            base.ExitState();

            if (Pathfinding.IsStrafing)
                Pathfinding.StopStrafing();

            if (CurrentGun)
                CurrentGun.OnTriggerReleased();
        }

        public override EnemyStateManager.EnemyState GetNextState()
        {
            if (CurrentGun.BulletsRemaining <= 0)
                return EnemyStateManager.EnemyState.Reloading;

            if (ChangeInHealth > 0.5f)
            {
                return EnemyStateManager.EnemyState.Hiding;
            }

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

            var distanceToPlayer = Vector2.Distance(Transform.position, Perception.player.transform.position);
            if (distanceToPlayer is <= MinPistolDistance or >= MaxPistolDistance)
            {
                float minDistance = MinPistolDistance, maxDistance = MaxPistolDistance;
                switch (CurrentGun)
                {
                    case ARGun:
                        minDistance = MinARDistance;
                        maxDistance = MaxARDistance;
                        break;
                    case Sniper:
                        minDistance = MinSniperDistance;
                        maxDistance = MaxSniperDistance;
                        break;
                }

                var direction = -(Perception.player.transform.position - Transform.position).normalized;
                var randomValidDistance = Random.Range(minDistance, maxDistance) - distanceToPlayer;

                var pointToMoveAt = Transform.position + direction * randomValidDistance;
                Pathfinding.MoveTo(pointToMoveAt);
            }
            else if (ChangeInHealth > 0.05f && !Pathfinding.IsStrafing)
            {
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
}