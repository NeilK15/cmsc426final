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

        /* Had to comment out this line because keeping the gameObject loaded broke the 
         *  scene management (going back and forth between the start scene and main game) 
         */
        // DontDestroyOnLoad(gameObject);

        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        //you can put a loading screen here before spawning the game if needed

        GameMode gameMode;
        GameState gameState;
        if (gameModePrefab && gameStatePrefab)
        {
            gameMode = Instantiate(gameModePrefab).GetComponent<GameMode>();
            gameState = Instantiate(gameStatePrefab).GetComponent<GameState>();

            GameAccess.RegisterGameMode(gameMode);
            GameAccess.RegisterGameState(gameState);

            StartCoroutine(gameMode.Init());
            StartCoroutine(gameState.Init());

        }
        onInitialized?.Invoke();
        yield return null;
    }
}
