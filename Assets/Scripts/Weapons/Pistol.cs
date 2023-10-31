using System.Collections;

namespace Weapons
{
    public class Pistol : Gun
    {
        public override GunType Type => GunType.Knife;
        public override FireMode[] FireModes => new[] { FireMode.Single };
        
        protected override IEnumerator Fire()
        {
            FireBullet();
            
            yield break;
        }
    }
}