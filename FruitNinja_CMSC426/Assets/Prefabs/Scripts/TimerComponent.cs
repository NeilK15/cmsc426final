using UnityEngine;
using UnityEngine.Events;

public class TimerComponent : MonoBehaviour
{
    public UnityEvent onSecondTick;
    public UnityEvent onTimerEnd;

    public float RemainingTime { get; private set; }

    private int lastSecond = -1;

    private void Awake()
    {
        if (onSecondTick == null) onSecondTick = new UnityEvent();
        if (onTimerEnd == null) onTimerEnd = new UnityEvent();
    }

    private void Update()
    {
        if (RemainingTime <= 0f) return;

        RemainingTime -= Time.deltaTime;
        int currentSecond = Mathf.FloorToInt(RemainingTime);

        if (currentSecond != lastSecond)
        {
            lastSecond = currentSecond;
            onSecondTick?.Invoke();
        }

        if (RemainingTime <= 0f)
        {
            RemainingTime = 0f;
            onTimerEnd?.Invoke();
        }
    }

    public void StartTimer(float seconds)
    {
        RemainingTime = seconds;
        lastSecond = Mathf.FloorToInt(seconds);
    }
}
