using Player;
using UnityEngine;
using Weapons;

namespace AI.StateMachine.EnemyStates
{
    public class EnemyShootingState : EnemyBaseState
    {
        private Transform Transform => stateManager.transform;

        private readonly CharacterDefenseStats _defenseStats;
        private bool _isStrafing;

        private Gun CurrentGun => Enemy.CurrentGun;

        public EnemyShootingState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager)
        {
            _isStrafing = false;
            _defenseStats = stateManager.gameObject.GetComponent<CharacterDefenseStats>();
            Perception.player.GetComponent<Rigidbody2D>();
        }

        public override void ExitState()
        {
            base.ExitState();

            if (CurrentGun)
                CurrentGun.OnTriggerReleased();
        }

        public override EnemyStateManager.EnemyState GetNextState()
        {
            if (CurrentGun.NormalizedBulletsRemaining <= 0.3f || _defenseStats.NormalizedHealth <= 0.6f)
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

            switch (CurrentGun)
            {
                case ARGun:
                case Pistol:
                    var bullets = Perception.GetBullets();
                    if (bullets.Length > 0)
                    {
                        var meanDirection = Vector2.zero;
                        var meanPosition = Vector2.zero;

                        foreach (var bullet in bullets)
                        {
                            meanDirection += (Vector2)bullet.right;
                            meanPosition += (Vector2)bullet.position;
                        }

                        meanDirection /= bullets.Length;
                        meanPosition /= bullets.Length;

                        if (Vector2.SignedAngle(Transform.position, meanPosition) < 0)
                            meanDirection = -meanDirection;

                        var strafePoint = (Vector2)Transform.position + meanDirection * 3.5f;
                        Pathfinding.Strafe(strafePoint);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}