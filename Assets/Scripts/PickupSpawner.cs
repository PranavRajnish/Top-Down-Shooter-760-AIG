using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    private float minSpawnTime = 30f;
    [SerializeField]
    private float maxSpawnTime = 120f;
    [SerializeField]
    private GameObject pickupPrefab;

    private float spawnTimer;
    private Pickup pickup;
    private bool _pickupSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }

        if(spawnTimer <= 0 && !_pickupSpawned)
        {
            _pickupSpawned=true;
            GameObject pickupGO = GameObject.Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            pickup = pickupGO.GetComponent<Pickup>();
            if(pickup != null )
            {
                pickup.onPickupCollected += OnPickupCollected;
            }
        }
    }

    private void OnPickupCollected()
    {
        pickup.onPickupCollected -= OnPickupCollected;
        spawnTimer = spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
        _pickupSpawned = false;
    }
}
