using Weapons;

namespace AI.StateMachine.EnemyStates
{
    public class EnemyReloadingState : EnemyBaseState
    {
        private Gun CurrentGun => Enemy.CurrentGun;
        public EnemyReloadingState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager) { }

        public override void EnterState()
        {
            CurrentGun.OnReloadPressed();
        }

        public override EnemyStateManager.EnemyState GetNextState()
        {
            if (CurrentGun.BulletsRemaining <= 0)
                return StateKey;

            if (Perception.CanSeePlayer)
                return EnemyStateManager.EnemyState.Shooting;

            return EnemyStateManager.EnemyState.FindPlayer;
        }
    }
}
