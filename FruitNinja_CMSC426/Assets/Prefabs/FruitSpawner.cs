using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private Fruit[] fruitPrefabs;
    [SerializeField] private float minForce = 10f;
    [SerializeField] private float maxForce = 15f;

    [SerializeField] private float waveDelay = 2f;
    [SerializeField] private int fruitsPerWave = 3;
    [SerializeField] private int spawnDivisions = 5;

    private List<Fruit> activeFruits = new();
    private Vector3[] spawnPoints;
    private Coroutine spawnLoopRoutine;

    private void Start()
    {
        ConfigureAsDeathZone();
        CalculateSpawnPoints();
        spawnLoopRoutine = StartCoroutine(SpawnLoop());
    }
    public void End()
    {
        if (spawnLoopRoutine != null)
            StopCoroutine(spawnLoopRoutine);

        foreach (var fruit in activeFruits)
        {
            if (fruit != null)
            {
                fruit.OnFruitDeath -= HandleFruitDeath;
                Destroy(fruit.gameObject); // or call fruit.ForceKill() if you have one
            }
        }

        activeFruits.Clear();
    }

    private void ConfigureAsDeathZone()
    {
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float y = cam.transform.position.y - cam.orthographicSize - 2f;

        transform.position = new Vector3(cam.transform.position.x, y, 0f);
        transform.tag = "DeathZone";

        BoxCollider col = GetComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = new Vector3(camWidth * 10f, 1f, 100f); // slightly wider than screen
        col.center = Vector3.zero;
    }

    private void CalculateSpawnPoints()
    {
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float y = cam.transform.position.y - cam.orthographicSize - 1f;

        spawnPoints = new Vector3[spawnDivisions];
        for (int i = 0; i < spawnDivisions; i++)
        {
            float x = cam.transform.position.x - camWidth / 2f + camWidth * (i + 0.5f) / spawnDivisions;
            spawnPoints[i] = new Vector3(x, y, 0f);
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnWave();
            yield return new WaitUntil(() => activeFruits.Count == 0);
            yield return new WaitForSeconds(waveDelay);
        }
    }

    private void SpawnWave()
    {
        for (int i = 0; i < fruitsPerWave; i++)
        {
            Vector3 spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Fruit prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            Fruit instance = Instantiate(prefab, spawn, Quaternion.identity);
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
