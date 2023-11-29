using UnityEngine;
using Weapons;

public class Enemy : MonoBehaviour
{
    public delegate void DEnemyDestroyed();

    public DEnemyDestroyed EnemyDestroyed;

    [SerializeField] private Gun currentGun;

    public Gun CurrentGun => currentGun;

    private void OnDestroy()
    {
        EnemyDestroyed?.Invoke();
    }
}