using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private Fruit[] fruitPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float minForce = 10f;
    [SerializeField] private float maxForce = 15f;

    [SerializeField] private float waveDelay = 2f;
    [SerializeField] private int fruitsPerWave = 3;

    private List<Fruit> activeFruits = new();

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnWave();

            // Wait until all fruits die
            yield return new WaitUntil(() => activeFruits.Count == 0);
            yield return new WaitForSeconds(waveDelay);
        }
    }

    private void SpawnWave()
    {
        for (int i = 0; i < fruitsPerWave; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Fruit prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            Fruit instance = Instantiate(prefab, spawn.position, Quaternion.identity);
            float forceY = Random.Range(minForce, maxForce);
            float forceX = Random.Range(-2f, 2f);
            instance.Launch(new Vector2(forceX, forceY));

            instance.OnFruitDeath += HandleFruitDeath;
            activeFruits.Add(instance);
        }
    }

    private void HandleFruitDeath(Fruit fruit)
    {
        fruit.OnFruitDeath -= HandleFruitDeath;
        activeFruits.Remove(fruit);
    }
}
