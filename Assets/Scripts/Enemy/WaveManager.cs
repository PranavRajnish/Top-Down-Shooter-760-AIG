using System.Collections.Generic;
using AI;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemyPrefabs;
    [SerializeField]
    private List<Transform> transforms = new List<Transform>();
    [SerializeField]
    PolygonCollider2D baseCollider;
    [SerializeField]
    GameObject player;
    [SerializeField]
    private TextMeshProUGUI waveText;
    [SerializeField]
    private bool debugModeOn = false;

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
        waveText.SetText("Wave: " + currentWave);
        enemiesToSpawn = currentWave * 2;
        enemiesLeft = enemiesToSpawn;

        for(int i = 0; i < enemiesToSpawn; i++)
        {
            int spawnIndex = Random.Range(0, transforms.Count);
            int typeIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyInstance = Instantiate(enemyPrefabs[typeIndex], transforms[spawnIndex].position, transforms[spawnIndex].rotation);

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

            EnemyStateManager enemyStateManager = enemyInstance.GetComponent<EnemyStateManager>();
            if(enemyStateManager != null )
            {
                enemyStateManager.debugStateOn = debugModeOn;
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
