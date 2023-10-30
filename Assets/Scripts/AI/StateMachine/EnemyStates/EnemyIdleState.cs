using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager) { }

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
    }

    public override void ExitState()
    {
        
    }

    public override EnemyStateManager.EnemyState GetNextState()
    {
        if (Perception.CanSeePlayer)
            return EnemyStateManager.EnemyState.Shooting;
        
        return stateKey;
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

    public override void UpdateState()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
