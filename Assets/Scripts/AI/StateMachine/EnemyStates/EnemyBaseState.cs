namespace AI.StateMachine.EnemyStates
{
    public abstract class  EnemyBaseState : BaseState<EnemyStateManager.EnemyState>
    {
        protected Enemy Enemy { get; private set; }
        protected EnemyPathfinding Pathfinding => stateManager.Pathfinding;
        protected readonly EnemyStateManager stateManager;
        protected EnemyPerception Perception => stateManager.Perception;

        protected EnemyBaseState(EnemyStateManager.EnemyState state, EnemyStateManager stateManager) : base(state)
        {
            this.stateManager = stateManager;
            Enemy = this.stateManager.gameObject.GetComponent<Enemy>();
        }
        public override void ExitState()
        {
            stateManager.previousState = StateKey;
        }
    }
}
