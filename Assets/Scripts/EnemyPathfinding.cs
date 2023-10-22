using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class EnemyPathfinding : MonoBehaviour
{
    public delegate void DReachedEndOfPath();
    public static DReachedEndOfPath ReachedEndOfPath;

    [SerializeField]
    private Vector2 target;
    [SerializeField]
    private float speed = 200f;
    [SerializeField]
    private float nextWaypointDistance = 3f;
    [SerializeField]
    private Transform EnemySprite;

    Path path;
    int currentWaypoint = 0;
    bool bReachedEndOfPath = false;
    bool bIsPathfinding = false;


    private Seeker seeker;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        //rb = GetComponent<Rigidbody2D>();

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null || !bIsPathfinding)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            ReachedEndOfPath?.Invoke();
            bReachedEndOfPath = true;
            return;
        }
        else
        {
            
            bReachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2) transform.position).normalized;
        Vector2 velocity = direction.normalized * speed * Time.deltaTime;

        FaceTarget(velocity);
        //FaceTarget((Vector3)target - transform.position);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
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
        bReachedEndOfPath = false;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    
    public void StopCalculatingPath()
    {
        bIsPathfinding=false;
        CancelInvoke("UpdatePath");
    }

    private void UpdatePath()
    {
        if (seeker.IsDone() && !bReachedEndOfPath)
        {
            seeker.StartPath(transform.position, target, OnPathFound);
        }
    }

    private void OnPathFound(Path p)
    {
        if(!p.error)
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
            EnemySprite.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void OnDrawGizmos()
    {
        if(!bIsPathfinding) { return; }
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target, 0.5f);
    }
}
