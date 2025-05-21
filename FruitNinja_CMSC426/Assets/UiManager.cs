using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{

    #region Singleton
    public static UiManager Instance { get; set; }

    void Awake()
    {
        if (Instance != null)
            Debug.LogError("More than one UIManager instance");

        Instance = this;
    }
    #endregion

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject endMenu;

    [SerializeField]
    private TextMeshProUGUI score;

    private bool isPaused;
    private GameState state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = GameAccess.GetGameState();
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                // Pausing game
                Debug.Log("Pause game");
                Time.timeScale = 0f;
                isPaused = true;
                pauseMenu.SetActive(true);

                // Updating the score
                int newScore = state.GetScore();
                score.text = "Score: " + newScore;
            }
        }
    }

    public void Resume()
    {
        // Resuming game
        Debug.Log("Return to game");
        Time.timeScale = 1.0f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void Exit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void EndGame()
    {
        Time.timeScale = 0f;
        endMenu.SetActive(true);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }
}
