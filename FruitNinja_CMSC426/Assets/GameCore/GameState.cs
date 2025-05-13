using UnityEngine;

public class GameState : MonoBehaviour
{
    private TimerComponent timerComponent;

    void Start()
    {
        GameAccess.RegisterGameState(this);
        timerComponent = gameObject.AddComponent<TimerComponent>();
        timerComponent.StartTimer(60f);
    }

    public float GetTime() => timerComponent.RemainingTime;
    public int GetScore() => timerComponent.Score;
    public void AddScore(int amount) => timerComponent.Score += amount;
}