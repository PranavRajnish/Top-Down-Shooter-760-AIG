using Player;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public PlayerWeaponControl WeaponHolder { get; private set; }
        
        public abstract WeaponType Type { get; }
        public abstract FireMode[] FireModes { get; }

        public abstract void OnAttackStart();
        public abstract void OnAttackEnd();
    }
}