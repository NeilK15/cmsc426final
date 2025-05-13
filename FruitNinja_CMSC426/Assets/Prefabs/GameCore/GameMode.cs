using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour
{
    [SerializeField] private GameObject controllerPrefab;

    private void Awake()
    {
        GameAccess.RegisterGameMode(this);
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        var spawn = FindFirstObjectByType<PlayerSpawn>();
        if (controllerPrefab && spawn != null)
        {
            Instantiate(controllerPrefab, spawn.transform.position, spawn.transform.rotation);
            yield return null;
        }
        else
        {
            Debug.LogWarning("GameMode: Missing ControllerPrefab or PlayerSpawn.");
        }
    }
}
