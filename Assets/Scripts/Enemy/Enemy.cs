using UnityEngine;
using Weapons;

public class Enemy : MonoBehaviour
{
    public delegate void DEnemyDestroyed();

    public DEnemyDestroyed EnemyDestroyed;

    public Gun currentGun;

    public Gun CurrentGun => currentGun;

    private void OnDestroy()
    {
        EnemyDestroyed?.Invoke();
    }
}