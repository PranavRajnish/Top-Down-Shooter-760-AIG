using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private List<Transform> transforms = new List<Transform>();
    [SerializeField]
    PolygonCollider2D baseCollider;
    [SerializeField]
    GameObject player;

    public int currentWave = 1;
    public int enemiesToSpawn;

    private int enemiesLeft;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("GenerateWave", 10);
    }

    private void GenerateWave()
    {
        enemiesToSpawn = currentWave * 2;
        enemiesLeft = enemiesToSpawn;

        for(int i = 0; i < enemiesToSpawn; i++)
        {
            int index = Random.Range(0, transforms.Count - 1);
            GameObject enemyInstance = Instantiate(enemyPrefab, transforms[index].position, transforms[index].rotation);

            EnemyPerception enemyPerception = enemyInstance.GetComponentInChildren<EnemyPerception>();
            if (enemyPerception != null)
            {
                enemyPerception.player = player;
            }

            Enemy enemyComponent = enemyInstance.GetComponent<Enemy>();
            if(enemyComponent != null )
            {
                enemyComponent.EnemyDestroyed += OnEnemyDestroyed;
            }
        }
    }

    private void OnEnemyDestroyed()
    {
        enemiesLeft -= 1;
        if (enemiesLeft <= 0)
        {
            currentWave += 1;
            Invoke("GenerateWave", 10);
        }
    }
}
