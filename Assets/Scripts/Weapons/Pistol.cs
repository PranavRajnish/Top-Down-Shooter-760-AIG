namespace Weapons
{
    public class Pistol : Weapon
    {
        public override WeaponType Type => WeaponType.Knife;
        public override FireMode[] FireModes => new[] { FireMode.Single };

        public override void OnAttackStart()
        {
            throw new System.NotImplementedException();
        }

        public override void OnAttackEnd()
        {
            throw new System.NotImplementedException();
        }
    }
}