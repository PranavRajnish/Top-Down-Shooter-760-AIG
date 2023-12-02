using UnityEngine;

namespace AI
{
    public class EnemyPerception : MonoBehaviour
    {
        [SerializeField] private float fovAngle = 45f;
        [SerializeField] private float sightDistance = 10f;
        [SerializeField] private LayerMask sightObstacles;

        public GameObject player;

        public bool CanSeePlayer { get; private set; }

        private Transform _parent;
        private float _yOffset;
        private Vector2 _bulletPerceptionBoxSize;

        // Start is called before the first frame update
        void Start()
        {
            var owner = GetComponentInParent<Enemy>();
            _parent = owner.transform;

            var ownerCollider = _parent.GetComponent<BoxCollider2D>();
            _yOffset = (sightDistance + ownerCollider.size.y) / 2;
            _bulletPerceptionBoxSize = new Vector2(ownerCollider.bounds.size.x, sightDistance);
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
                CanSeePlayer = false;
                return;
            }


            // Checking Angle
            Vector2 dirToPlayer = player.transform.position - transform.position;
            dirToPlayer.Normalize();
            if (!(Vector2.Angle(_parent.right, dirToPlayer) < fovAngle / 2f))
            {
                CanSeePlayer = false;
                return;
            }

            // Checking if sight is blocked
            RaycastHit2D raycastHit = Physics2D.Raycast(_parent.position, dirToPlayer, distanceToPlayer, sightObstacles.value);
            if (raycastHit.collider != null)
            {
                CanSeePlayer = false;
                return;
            }

            CanSeePlayer = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.parent.right * sightDistance);

            Gizmos.color = CanSeePlayer ? Color.red : Color.yellow;
            var halfFOV = fovAngle / 2f;
            var coneDirection = transform.parent.forward;
            var upRayRotation = Quaternion.AngleAxis(-halfFOV, coneDirection);
            var downRayRotation = Quaternion.AngleAxis(halfFOV, coneDirection);

            var upRayDirection = upRayRotation * transform.right * sightDistance;
            var downRayDirection = downRayRotation * transform.right * sightDistance;

            Gizmos.DrawRay(transform.position, upRayDirection);
            Gizmos.DrawRay(transform.position, downRayDirection);
            Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);
        }

        public float GetSightDistance()
        {
            return sightDistance;
        }
    }
}