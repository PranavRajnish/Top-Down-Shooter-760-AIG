using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyPerception : MonoBehaviour
{
    public delegate void DPlayerFound();
    public static DPlayerFound PlayerFound;

    public delegate void DPlayerLost();
    public static DPlayerLost PlayerLost;

    [SerializeField]
    private float fovAngle = 45f;
    [SerializeField]
    private float sightDistance = 10f;
    [SerializeField]
    private LayerMask sightObstacles;

    public GameObject player;

    bool bCanSeePlayer = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();   
    }

    void CheckForPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        // Checking Distance
        if (!(distanceToPlayer < sightDistance))
        {
            if (bCanSeePlayer)
                PlayerLost?.Invoke();

            bCanSeePlayer = false;
            return;
        }
            

        // Checking Angle
        Vector2 dirToPlayer = player.transform.position - transform.position; 
        dirToPlayer.Normalize();
        if(!(Vector2.Angle(transform.parent.right, dirToPlayer) < fovAngle/2f))
        {
            if (bCanSeePlayer)
                PlayerLost?.Invoke();
            bCanSeePlayer = false;
            return;
        }

        // Checking if sight is blocked
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.parent.position, dirToPlayer, distanceToPlayer, sightObstacles.value);
        if(raycastHit.collider != null)
        {
            if (bCanSeePlayer)
                PlayerLost?.Invoke();
            bCanSeePlayer = false;
            return;
        }

        bCanSeePlayer = true;
        PlayerFound?.Invoke();
        Debug.Log("Player");
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.parent.right * sightDistance);

        if (bCanSeePlayer)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.yellow;
        float halfFOV = fovAngle / 2f;
        Vector3 coneDirection = transform.parent.forward;
        Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV, coneDirection);
        Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV, coneDirection);

        Vector3 upRayDirection = upRayRotation * transform.right * sightDistance;
        Vector3 downRayDirection = downRayRotation * transform.right * sightDistance;

        Gizmos.DrawRay(transform.position, upRayDirection);
        Gizmos.DrawRay(transform.position, downRayDirection);
        Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);
    }
}
