using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerDefenseStats : PlayerComponent
    {
        [SerializeField] private float playerHealth = 100f;
        [SerializeField] private float playerArmor = 50f;

        private const float MaxPlayerHealth = 100f;
        private const float MaxPlayerArmor = 50f;

        public void OnDamageDealt(Bullet bullet)
        {
            if (playerArmor > 0)
            {
                if (playerArmor < bullet.Damage)
                    playerArmor = 0;
                else
                    playerArmor -= bullet.Damage;
            }
            else
            {
                playerHealth -= bullet.Damage;

                if (playerHealth <= 0)
                    new PlayerDeathEvent(PlayerID).Raise();
            }
        }
    }

    public class PlayerDeathEvent : GameEvent
    {
        public int PlayerID;

        public PlayerDeathEvent(int playerID)
        {
            PlayerID = playerID;
        }
    }
}