using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GameInstance : MonoBehaviour
{
    public static GameInstance Instance { get; private set; }

    [SerializeField] private GameObject gameStatePrefab;
    [SerializeField] private GameObject gameModePrefab;

    public UnityEvent onInitialized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        GameAccess.RegisterGameInstance(this);
        DontDestroyOnLoad(gameObject);

        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        if (gameModePrefab)
        {
            Instantiate(gameModePrefab);
            yield return null;
        }

        if (gameStatePrefab)
        {
            Instantiate(gameStatePrefab);
            yield return null;
        }


        onInitialized?.Invoke();
    }
}
