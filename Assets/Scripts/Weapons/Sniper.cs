using System.Collections;

namespace Weapons
{
    public class Sniper : Gun
    {
        public override GunType Type => GunType.Sniper;
        public override FireMode[] FireModes => new[] { FireMode.Single };
        protected override IEnumerator Fire()
        {
            FireBullet();
            
            yield break;
        }
    }
}