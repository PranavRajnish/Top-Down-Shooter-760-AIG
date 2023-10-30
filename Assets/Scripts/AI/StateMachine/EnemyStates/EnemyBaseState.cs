public abstract class  EnemyBaseState : BaseState<EnemyStateManager.EnemyState>
{
    protected EnemyPathfinding Pathfinding => stateManager.Pathfinding;
    protected readonly EnemyStateManager stateManager;
    protected EnemyPerception Perception => stateManager.Perception;

    public EnemyBaseState(EnemyStateManager.EnemyState state, EnemyStateManager stateManager) : base(state)
   {
        this.stateManager = stateManager;
   }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        
    }
}
