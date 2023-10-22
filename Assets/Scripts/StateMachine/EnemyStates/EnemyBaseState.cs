using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  EnemyBaseState : BaseState<EnemyStateManager.EnemyState>
{
    protected EnemyPathfinding pathfinding;
   public EnemyBaseState(EnemyStateManager.EnemyState state, EnemyPathfinding pathfinding) : base(state)
   {
        this.pathfinding = pathfinding;
   }


}
