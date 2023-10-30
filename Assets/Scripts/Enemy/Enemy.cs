using UnityEngine;
using Weapons;

public class Enemy : MonoBehaviour
{
    public delegate void DEnemyDestroyed();
    public DEnemyDestroyed EnemyDestroyed;

    public Gun currentGun; 

    private void OnDestroy()
    {
        EnemyDestroyed?.Invoke();
    }
}
