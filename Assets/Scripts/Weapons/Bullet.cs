using Player;
using UnityEngine;

namespace Weapons
{
    public class Bullet : MonoBehaviour
    {
        public float Damage => _damage;
        [SerializeField] private float bulletSpeed = 5f;

        private Vector2 _currentDirection = Vector2.one;
        private float _damage = 10;

        public Bullet Init(Gun parentGun)
        {
            _damage = parentGun.BulletDamage;

            return this;
        }

        public void OnBulletFired(Vector2 startingPos, Vector2 direction)
        {
            transform.position = startingPos;
            _currentDirection = direction;

            gameObject.SetActive(true);
        }

        private void Update()
        {
            transform.position += (Vector3)_currentDirection * (Time.deltaTime * bulletSpeed);
        }

        private void OnCollisionEnter(Collision other)
        {
            var playerDefenseStats = other.gameObject.GetComponent<PlayerDefenseStats>();

            if (playerDefenseStats)
            {
                // Deal damage.
                playerDefenseStats.OnDamageDealt(this);
            }

            // var wall = other.gameObject.GetComponent<Wall>();
            //
            // if (wall)
            // {
            //     // Show bullet marks.
            // }

            gameObject.SetActive(false);
        }
    }
}