using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] protected GameObject coinPrefab;
    [SerializeField] private Vector3 _randomArea = new Vector3(10, 10, 0);
    public static float spawnInterval = 5f;
    public static int maxCoin = 10;
    [SerializeField] protected float spawnDistance = 1f;

    private List<GameObject> activeCoins = new List<GameObject>();

    public int NumberCoin
    {
        get
        {
            activeCoins.RemoveAll(coin => coin == null || !coin.activeInHierarchy);
            return activeCoins.Count;
        }
    }

    private float spawnTimer = 0f;

    void FixedUpdate()
    {
        if (NumberCoin < maxCoin)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnCoin();
                spawnTimer = 0f;
            }
        }
    }

    private void SpawnCoin()
    {
        float randomX = Random.Range(-_randomArea.x / 2f, _randomArea.x / 2f);
        float randomY = Random.Range(-_randomArea.y / 2f, _randomArea.y / 2f);
        float randomZ = Random.Range(-_randomArea.z / 2f, _randomArea.z / 2f);

        Vector3 spawnPosition = transform.position + new Vector3(randomX, randomY, randomZ) * spawnDistance;

        GameObject newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        activeCoins.Add(newCoin);
    }
}