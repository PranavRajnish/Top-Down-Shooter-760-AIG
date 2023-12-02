using Player;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArmorPickup : Pickup
{
    [SerializeField]
    private float armorAmount = 25f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
        CharacterDefenseStats stats = collision.gameObject.GetComponent<CharacterDefenseStats>();
        if (stats && stats.IsPlayer)
        {
            if(stats.NormalizedArmor != 1)
            {
                stats.AddArmor(armorAmount);
                onPickupCollected?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
