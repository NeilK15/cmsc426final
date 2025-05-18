using UnityEngine;
using TMPro;

public class ScoreWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        GameState state = GameAccess.GetGameState();
        if (state != null)
        {
            state.OnScoreChanged.AddListener(UpdateScore);
            UpdateScore(state.GetScore()); // set initial value
        }
    }

    private void UpdateScore(int newScore)
    {
        if(scoreText) scoreText.text = $"Score: {newScore}";
    }
}
