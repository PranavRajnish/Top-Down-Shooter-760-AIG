using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public delegate void DOnPickupCollected();
    public DOnPickupCollected onPickupCollected;

    protected abstract void OnTriggerEnter2D(Collider2D collision);
}
