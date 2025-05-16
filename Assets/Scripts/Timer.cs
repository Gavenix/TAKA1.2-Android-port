using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    public float waveDuration = 30f;
private float timeLeft;

private void Start()
{
    timeLeft = 0;
}

public void StartTimer()
{
    timeLeft = waveDuration;
    StartCoroutine(TimerCountdown());
}

private IEnumerator TimerCountdown()
{
    while (timeLeft > 0)
    {
        timeLeft -= Time.deltaTime;
        yield return null;
    }
}

public float GetTime()
{
    return timeLeft;
}
}

