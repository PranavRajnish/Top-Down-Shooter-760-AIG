using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Sniper : Gun
    {
        public override GunType Type => GunType.Sniper;
        public override FireMode[] FireModes => new[] { FireMode.Single };

        private const float MinFireRate = 0.8f;
        private float _lastShootTime = float.MinValue;

        protected override IEnumerator Fire()
        {
            var currentTime = Time.time;
            if (currentTime - _lastShootTime < MinFireRate) yield break;

            _lastShootTime = currentTime;
            FireBullet();
        }
    }
}