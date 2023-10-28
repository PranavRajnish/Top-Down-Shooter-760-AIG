using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void DEnemyDestroyed();
    public DEnemyDestroyed EnemyDestroyed;

    private void OnDestroy()
    {
        EnemyDestroyed?.Invoke();
    }

}
