using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class ARGun : Gun
    {
        public override GunType Type => GunType.AR;
        public override FireMode[] FireModes => new[] { FireMode.Auto };

        [SerializeField] private float fireRate;

        protected override void Start()
        {
            base.Start();

            CurrentFireMode = FireModes[0];
        }

        protected override IEnumerator Fire()
        {
            while (true)
            {
                FireBullet();
                
                yield return new WaitForSeconds(1/fireRate);
            }
        }
    }
}