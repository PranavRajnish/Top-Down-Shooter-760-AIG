using System;
using Player;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Bullet : MonoBehaviour
    {
        public float Damage => _damage;
        [SerializeField] private float bulletSpeed = 5f;
        [SerializeField] private int obstacleLayerInt;

        private Transform _muzzle;
        private Vector2 _currentDirection;
        private float _damage = 10;

        private DateTime _startTime;

        public Bullet Init(Gun parentGun)
        {
            _damage = parentGun.BulletDamage;
            _muzzle = parentGun.Muzzle;

            return this;
        }

        private void OnEnable()
        {
            _startTime = DateTime.Now;
        }

        public void OnBulletFired()
        {
            transform.position = _muzzle.position;
            _currentDirection = _muzzle.up;

            gameObject.SetActive(true);
        }

        private void Update()
        {
            if ((DateTime.Now - _startTime).Seconds > 2f)
                gameObject.SetActive(false);

            transform.position += (Vector3)_currentDirection * (Time.deltaTime * bulletSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var playerDefenseStats = other.gameObject.GetComponent<CharacterDefenseStats>();

            if (playerDefenseStats)
            {
                // Deal damage.
                playerDefenseStats.OnDamageDealt(this);
                gameObject.SetActive(false);
            }

            // var wall = other.gameObject.GetComponent<Wall>();
            //
            // if (wall)
            // {
            //     // Show bullet marks.
            // }
            if (other.gameObject.layer == obstacleLayerInt)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == obstacleLayerInt)
            {
                gameObject.SetActive(false);
            }
        }
    }
}