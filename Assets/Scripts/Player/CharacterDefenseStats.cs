using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Player
{
    public class CharacterDefenseStats : PlayerComponent
    {
        [SerializeField] private bool isPlayer;
        [SerializeField] private float health = 100f;
        [SerializeField] private float armor = 50f;

        private const float MaxPlayerHealth = 100f;
        private const float MaxPlayerArmor = 50f;

        public void OnDamageDealt(Bullet bullet)
        {
            if (armor > 0)
            {
                if (armor < bullet.Damage)
                    armor = 0;
                else
                    armor -= bullet.Damage;
            }
            else
            {
                health -= bullet.Damage;

                if (health <= 0)
                {
                    if(!isPlayer)
                        Destroy(gameObject);
                    else
                    {
                        
                    }
                    new CharacterDeathEvent(CharacterID).Raise();
                }
            }
        }
    }

    public class CharacterDeathEvent : GameEvent
    {
        public int CharacterID;

        public CharacterDeathEvent(int characterID)
        {
            CharacterID = characterID;
        }
    }
}