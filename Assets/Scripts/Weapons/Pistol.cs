using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Pistol : Gun
    {
        public override GunType Type => GunType.Knife;
        public override FireMode[] FireModes => new[] { FireMode.Single };

        private const float MinFireRate = 0.5f;
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