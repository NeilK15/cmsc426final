using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{
    public TimerComponent timerComponent;
    public int Score { get; private set; } = 0;

    public UnityEvent<int> OnScoreChanged = new(); // event: new score passed

    public IEnumerator Init()
    {
        timerComponent = gameObject.AddComponent<TimerComponent>();
        timerComponent.StartTimer(60f);
        yield return null;
    }

    public float GetTime() => timerComponent.RemainingTime;
    public int GetScore() => Score;

    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged.Invoke(Score);
    }
    public void ResetScore()
    {
        Score = 0;
    }
}
