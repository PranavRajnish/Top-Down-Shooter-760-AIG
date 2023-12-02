using System;
using UnityEngine;
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

        public float NormalizedHealth => (health + armor) / MaxPlayerHealth;

        public float NormalizedHealthOnly => health/MaxPlayerHealth;
        public float NormalizedArmor => armor/MaxPlayerArmor;

        public event Action OnCharacterHit;

        public event Action OnCharacterStatUpdate;

        public bool IsPlayer { get { return isPlayer; } }

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
                    
                    new CharacterDeathEvent(CharacterID).Raise();
                }
            }
            
            OnCharacterHit?.Invoke();
            OnCharacterStatUpdate?.Invoke();
        }

        public void AddHealth(float amt)
        {
            health += amt;
            OnCharacterStatUpdate?.Invoke();
        }

        public void AddArmor(float amt)
        {
            armor += amt;
            OnCharacterStatUpdate?.Invoke();
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