using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private Vector2 target;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private Transform EnemySprite;
    [SerializeField] private Vector3 TargetPosition;

    [Header("Cover finding properties")]
    [SerializeField] private float initialCoverSphereRadius = 4f;
    [SerializeField] private float maximumCoverSphereRadius = 12f;
    [SerializeField] private float coverSphereRadiusIncrement = 4f;
    [SerializeField] private int raysPerCoverSphere = 12;
    [SerializeField] private float maximumDistanceToCover = 10f;
    [SerializeField] private LayerMask obstacleMask;

    public delegate void DHidingAttemptFinished();
    public DHidingAttemptFinished hidingAttemptFinished;

    private bool bIsTryingToHide = false;

    Path path;
    int currentWaypoint = 0;
    public bool ReachedEndOfPath { get; private set; }
    bool bIsPathfinding = false;


    private Seeker _seeker;

    private Seeker Seeker
    {
        get
        {
            if (!_seeker)
                _seeker = GetComponent<Seeker>();
            
            return _seeker;
        }
    }


    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null || !bIsPathfinding)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            ReachedEndOfPath = true;
            
            return;
        }
        else
        {
            ReachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
        Vector2 velocity = direction.normalized * (speed * Time.deltaTime);

        FaceTarget(velocity);
        //FaceTarget((Vector3)target - transform.position);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //rb.AddForce(velocity); 
        float newX = transform.position.x + velocity.x * Time.deltaTime;
        float newY = transform.position.y + velocity.y * Time.deltaTime;
        transform.position = new Vector2(newX, newY);
    }

    public void CalculateNewPath(Vector2 targetPoint)
    {
        Debug.Log("New Target Set");
        target = targetPoint;
        bIsPathfinding = true;
        ReachedEndOfPath = false;
        //InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
        //Seeker.CancelCurrentPathRequest();
        UpdatePath();
    }

    public void StopCalculatingPath()
    {
        bIsPathfinding = false;
        //CancelInvoke(nameof(UpdatePath));
    }

    private void UpdatePath()
    {
        if (Seeker.IsDone())
        {
            Seeker.StartPath(transform.position, target, OnPathFound);
        }
    }

    private void OnPathFound(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FaceTarget(Vector3 velocity)
    {
        if (velocity.magnitude > 0)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    public void FindCover(Transform coverTarget)
    {
        float angleIncrement = 360 / raysPerCoverSphere;

        for(float r = initialCoverSphereRadius; r <= maximumCoverSphereRadius; r+=coverSphereRadiusIncrement)
        {
            float minDistance = Mathf.Infinity;
            Vector2 targetPoint = Vector2.zero;
            bool bFoundValidCoverSpot = false;
            for (int i = 0; i < raysPerCoverSphere; i++)
            {
                Quaternion vectorRotation = Quaternion.AngleAxis(angleIncrement * i, Vector3.forward);
                Vector2 rayStart = coverTarget.position + (r * (vectorRotation * Vector2.right));

                RaycastHit2D raycastHit;
                raycastHit = Physics2D.Raycast(rayStart, (Vector2)coverTarget.position - rayStart, r, obstacleMask.value);


                Debug.DrawRay(rayStart, (Vector2)coverTarget.position - rayStart, Color.gray, 5f);

                float DistanceToCover = Vector2.Distance(transform.position, raycastHit.point);
                if (raycastHit.collider != null && DistanceToCover <= maximumDistanceToCover && DistanceToCover < minDistance)
                {
                    bFoundValidCoverSpot = true;
                    targetPoint = raycastHit.point;
                    minDistance = Vector2.Distance(transform.position, raycastHit.point);
                }

            }

            if (bFoundValidCoverSpot)
            {
                CalculateNewPath(targetPoint);
                bIsTryingToHide = true;
                break;
            }
            else
            {
                hidingAttemptFinished?.Invoke();
            }
        }
       
    }

    private void OnDrawGizmos()
    {
        if (!bIsPathfinding)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target, 0.5f);
    }
}
