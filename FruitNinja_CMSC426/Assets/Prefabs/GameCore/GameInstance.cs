using UnityEngine;
using UnityEngine.Events;

public class GameInstance : MonoBehaviour
{
    public static GameInstance Instance { get; private set; }

    [SerializeField] private GameObject gameModePrefab;
    [SerializeField] private GameObject mainHUDPrefab;
    [SerializeField] private GameObject gameStatePrefab;

    public UnityEvent onInitialized; // Event called after all instantiations

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

        // Start coroutine to allow full initialization before firing event
        StartCoroutine(SpawnAndNotify());
    }

    private System.Collections.IEnumerator SpawnAndNotify()
    {
        if (gameModePrefab) Instantiate(gameModePrefab);
        yield return null;

        if (mainHUDPrefab) Instantiate(mainHUDPrefab);
        yield return null;

        if (gameStatePrefab) Instantiate(gameStatePrefab);
        yield return null;

        onInitialized?.Invoke(); // Notify subscribers UI can now spawn
    }
}
