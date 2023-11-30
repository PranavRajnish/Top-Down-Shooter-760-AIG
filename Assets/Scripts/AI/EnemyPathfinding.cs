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
    [SerializeField] private float maxPerpendicularDistance = 6f;
    [SerializeField] private float coverRayIncrement = 2f;
    [SerializeField] private float maximumDistanceToCover = 10f;
    [SerializeField] private LayerMask obstacleMask;

    public delegate void DHidingAttemptFinished();

    public DHidingAttemptFinished hidingAttemptFinished;

    private bool bIsTryingToHide = false;

    Path path;
    int currentWaypoint = 0;
    public bool ReachedEndOfPath { get; private set; }
    bool bIsPathfinding = false;

    public delegate void DReachedEndOfPath();

    public DReachedEndOfPath reachedEndOfPath;


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


    private Rigidbody2D _rigidbody;

    private Coroutine _strafingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null || !bIsPathfinding)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            ReachedEndOfPath = true;
            reachedEndOfPath?.Invoke();

            if (bIsTryingToHide)
            {
                bIsTryingToHide = false;
                hidingAttemptFinished?.Invoke();
            }

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

    public void FindCover(Transform coverTarget)
    {
        /* float angleIncrement = 360 / raysPerCoverSphere;
         for (float r = initialCoverSphereRadius; r <= maximumCoverSphereRadius; r += coverSphereRadiusIncrement)
         {
             float minDistance = Mathf.Infinity;
             Vector2 targetPoint = Vector2.zero;
             bool bFoundValidCoverSpot = false;
             for (int i = 0; i < raysPerCoverSphere; i++)
             {
                 Quaternion vectorRotation = Quaternion.AngleAxis(angleIncrement * i, Vector3.forward);
                 Vector2 rayStart = coverTarget.position + (r * (vectorRotation * Vector2.right));

                 // If ray start is in obstacle, should not be valid ray
                 Collider2D collider = Physics2D.OverlapCircle(rayStart, 0.5f, obstacleMask.value);
                 if (collider != null)
                 {
                     Debug.Log("Ray start in obstacle");
                     continue;
                 }

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
                 Debug.Log("No hiding spot found");
                 hidingAttemptFinished?.Invoke();
             }
         }*/


        Vector3 dir = coverTarget.position - transform.position;
        Vector3 perpendicular = Vector3.Cross(dir, new Vector3(0,0,1));
        perpendicular.Normalize();

        float minDistance = Mathf.Infinity;
        Vector2 targetPoint = Vector2.zero;
        bool bFoundValidCoverSpot = false;
        for (float d = -maxPerpendicularDistance; d <= maxPerpendicularDistance; d += coverRayIncrement)
        {
            
            Vector2 rayStart = (Vector2) transform.position + (d * (Vector2)perpendicular);

            Collider2D collider = Physics2D.OverlapCircle(rayStart, 0.5f, obstacleMask.value);
            if (collider != null)
            {
                Debug.Log("Ray start in obstacle");
                continue;
            }

            RaycastHit2D raycastHit;
            Vector2 rayDir = (Vector2)coverTarget.position - rayStart;
            raycastHit = Physics2D.Raycast(rayStart, rayDir, rayDir.magnitude, obstacleMask.value);
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
        }
        else
        {
            Debug.Log("No hiding spot found");
            hidingAttemptFinished?.Invoke();
        }
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

    private void OnDrawGizmos()
    {
        if (!bIsPathfinding)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 0.1f);

        if(path != null)
        {
            Gizmos.color = Color.yellow;
            foreach(Vector3 v3 in path.vectorPath)
            {
                Gizmos.DrawSphere(v3, 0.05f);
            }
        }

        if (_strafingCoroutine != null)
        {
            Gizmos.DrawSphere(_strafePoint, 0.05f);
        }
    }

    private Vector2 _strafePoint;

    public void Strafe(Vector2 destination)
    {
        if (_strafingCoroutine != null)
            StopCoroutine(_strafingCoroutine);

        _strafingCoroutine = StartCoroutine(StrafeRoutine(destination));
    }

    private IEnumerator StrafeRoutine(Vector2 destination)
    {
        StopCalculatingPath();
        Debug.Log("I'm strafing!!!!");
        var startTime = Time.time;
        var speed = Random.Range(3f, 5f);
        var direction = destination - (Vector2)transform.position;
        direction.Normalize();

        while (Vector2.Distance(transform.position, destination) > 0.25f || Time.time - startTime > 3f)
        {
            _rigidbody.velocity = speed * direction;
            yield return null;
        }
    }
}