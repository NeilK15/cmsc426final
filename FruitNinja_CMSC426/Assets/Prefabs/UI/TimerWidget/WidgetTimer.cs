using UnityEngine;
using TMPro;

public class WidgetTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private TimerComponent timer;

    private void Start()
    {
        GameState gameState = GameAccess.GetGameState();
        if (gameState == null) return;

        timer = gameState.GetComponent<TimerComponent>();
        if (timer == null) return;

        timer.onSecondTick.AddListener(UpdateTimerDisplay);
        timer.onTimerEnd.AddListener(UpdateTimerDisplay);

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timer == null) return;

        int minutes = Mathf.FloorToInt(timer.RemainingTime / 60f);
        int seconds = Mathf.FloorToInt(timer.RemainingTime % 60f);
        timerText.text = $"{minutes}:{seconds:00}";
    }
}
