using Player;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField]
    private float healthAmount = 25f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    { 
        CharacterDefenseStats stats = collision.gameObject.GetComponent<CharacterDefenseStats>();
        if (stats && stats.IsPlayer)
        {
            if(stats.NormalizedHealthOnly != 1)
            {
                stats.AddHealth(healthAmount);
                onPickupCollected?.Invoke();
                Destroy(gameObject);
            }
            
        }
    }
}
