using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Text timerText;
    public Slider graphSlider;
    public float maxDuration;
    public float accumulatedTime;
    public float startTime;
    private bool isTimerRunning;

    private void Start()
    {
        isTimerRunning = false;
        maxDuration = graphSlider.maxValue;
        timerText.text = "";
        graphSlider.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            float elapsed = Time.time - startTime;
            UpdateGraphBar(accumulatedTime + elapsed);
        }
    }

    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            startTime = Time.time;
            isTimerRunning = true;
            timerText.text = "";
            graphSlider.gameObject.SetActive(true);
        }
        else
        {
            StopTimer();
        }
    }

    public void StopTimer()
    {
        if (isTimerRunning)
        {
            float elapsedTime = Time.time - startTime;
            accumulatedTime += elapsedTime;
            isTimerRunning = false;
            UpdateTimerText(elapsedTime);
            UpdateGraphBar(accumulatedTime);
            timerText.text = "Bulb was ON for: " + FormatTime(elapsedTime);
            graphSlider.gameObject.SetActive(true);
        }
        else
        {
            accumulatedTime = 0f;
            graphSlider.value = 0f;
            timerText.text = "";
            graphSlider.gameObject.SetActive(true);
        }
    }

    private void UpdateTimerText(float elapsed)
    {
        timerText.text = "Elapsed Time: " + FormatTime(elapsed);
    }

    private void UpdateGraphBar(float elapsed)
    {
        float normalizedTime = elapsed / maxDuration;
        graphSlider.value = normalizedTime;
    }

    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateAccumulatedTime(string weekday, float time)
    {
        PlayerPrefs.SetFloat(weekday, time);
    }
}
