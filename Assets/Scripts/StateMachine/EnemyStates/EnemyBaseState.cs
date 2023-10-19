using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  EnemyBaseState : BaseState<EnemyStateManager.EnemyState>
{
   public EnemyBaseState(EnemyStateManager.EnemyState state) : base(state)
   {

   }
}
