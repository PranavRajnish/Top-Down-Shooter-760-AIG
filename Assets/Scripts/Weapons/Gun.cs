using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using Unity.Mathematics;
using UnityEngine;

namespace Weapons
{
    public abstract class Gun : MonoBehaviour
    {
        public PlayerWeaponControl WeaponWielder { get; private set; }
        public abstract GunType Type { get; }
        public abstract FireMode[] FireModes { get; }
        public FireMode CurrentFireMode { get; protected set; }
        public float BulletDamage => bulletDamage;
        public Transform Muzzle => muzzle;
        public int BulletsRemaining { get; private set; }

        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform muzzle;
        [SerializeField] private int magSize;
        [SerializeField] private float reloadTime = 10f;
        [SerializeField] private float bulletDamage;

        private List<Bullet> _poolOfBullets;
        private Coroutine _shootingCoroutine;
        private Coroutine _reloadCoroutine;
        private bool _triggerPulled;

        protected virtual void Start()
        {
            _poolOfBullets = new List<Bullet>();
            BulletsRemaining = magSize;
            bulletPrefab.gameObject.SetActive(false);
        }

        public void OnTriggerPulled()
        {
            if(_triggerPulled)
                return;
            
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);

            _triggerPulled = true;
            _shootingCoroutine = StartCoroutine(Fire());
        }

        protected abstract IEnumerator Fire();

        protected void FireBullet()
        {
            if (BulletsRemaining <= 0)
                return;

            if (_poolOfBullets.Count <= magSize && _poolOfBullets.All(bullet => bullet.gameObject.activeSelf))
            {
                _poolOfBullets.Add(Instantiate(bulletPrefab, muzzle.position, quaternion.identity).Init(this));
            }

            foreach (var bullet in _poolOfBullets.Where(bullet => !bullet.gameObject.activeSelf))
            {
                BulletsRemaining--;
                bullet.OnBulletFired();
                break;
            }
        }

        public void OnTriggerReleased()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);

            _triggerPulled = false;
        }

        public void OnReloadPressed()
        {
            if (_reloadCoroutine == null && BulletsRemaining < magSize)
                _reloadCoroutine = StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(reloadTime);

            BulletsRemaining = magSize;
            _reloadCoroutine = null;
        }
    }
}