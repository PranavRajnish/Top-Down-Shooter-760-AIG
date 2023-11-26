using UnityEngine;
using Weapons;

public class EnemyShootingState : EnemyBaseState
{
    private Gun _currentGun;
    private Rigidbody playerRB;
    private Transform Transform => stateManager.transform;

    public EnemyShootingState(EnemyStateManager.EnemyState state, EnemyStateManager enemyStateManager) : base(state, enemyStateManager)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enterred Shooting State");

        base.EnterState();

        _currentGun = stateManager.gameObject.GetComponent<Enemy>().currentGun;

        playerRB = Perception.player.GetComponent<Rigidbody>();
    }


    public override void ExitState()
    {
        base.ExitState();

        _currentGun.OnTriggerReleased();
    }

    public override EnemyStateManager.EnemyState GetNextState()
    {
        if (_currentGun.BulletsRemaining <= 0)
            return EnemyStateManager.EnemyState.Hiding;
        
        if (Perception.CanSeePlayer)
            return stateKey;

        return EnemyStateManager.EnemyState.FindPlayer;
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
        if (Perception.CanSeePlayer)
        {
            var direction = Perception.player.transform.position - _currentGun.Muzzle.position;
            Transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));

            _currentGun.OnTriggerPulled();

            if(_currentGun is ARGun)
            {
                Vector2 playerVelocity = playerRB.velocity;
                if (playerRB && playerVelocity.sqrMagnitude > 1f)
                {
                    Pathfinding.CalculateNewPath((Vector2)stateManager.transform.position + playerVelocity.normalized);
                }
            }
            
           
        }
    }
}