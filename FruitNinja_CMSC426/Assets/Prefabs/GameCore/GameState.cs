using UnityEngine;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{
    private TimerComponent timerComponent;
    public int Score { get; private set; } = 0;

    public UnityEvent<int> OnScoreChanged = new(); // event: new score passed

    private void Start()
    {
        GameAccess.RegisterGameState(this);
        timerComponent = gameObject.AddComponent<TimerComponent>();
        timerComponent.StartTimer(60f);
    }

    public float GetTime() => timerComponent.RemainingTime;
    public int GetScore() => Score;

    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged.Invoke(Score);
    }
}
