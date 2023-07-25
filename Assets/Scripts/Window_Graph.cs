using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private RectTransform labelTemplateX;
    [SerializeField] private RectTransform labelTemplateY;
    private float graphHeight;
    private TimerController timerController;
    private Dictionary<string, float> accumulatedTimes;

    private void Awake()
    {
        timerController = FindObjectOfType<TimerController>();
        accumulatedTimes = LoadAccumulatedTimes();

        List<float> valueList = new List<float>(accumulatedTimes.Values);
        List<string> weekdays = new List<string>(accumulatedTimes.Keys);

        ShowGraph(valueList, weekdays);
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0f); // Set y-position to 0
        return gameObject;
    }


    private void ShowGraph(List<float> valueList, List<string> weekdays)
    {
        graphHeight = graphContainer.sizeDelta.y;

        float xSize = graphContainer.sizeDelta.x / (valueList.Count + 1);

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = valueList[i] / timerController.maxDuration * graphHeight; // Calculate the y-position based on the accumulated time
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            CreateXAxisLabel(weekdays[i], xPosition);

            // Highlight the circle for the current day
            if (weekdays[i] == GetCurrentWeekday())
            {
                circleGameObject.GetComponent<Image>().color = Color.yellow;
            }
        }

        CreateYAxisLabels();
    }

    private float GetYPosition(float accumulatedTime)
    {
        float normalizedTime = accumulatedTime / timerController.maxDuration;
        return normalizedTime * graphHeight;
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

    private void CreateXAxisLabel(string labelText, float xPosition)
    {
        RectTransform labelTransform = Instantiate(labelTemplateX);
        labelTransform.SetParent(graphContainer, false);
        labelTransform.gameObject.SetActive(true);
        labelTransform.anchoredPosition = new Vector2(xPosition, -20f);
        labelTransform.GetComponent<Text>().text = labelText;
    }

    private void CreateYAxisLabels()
    {
        int separatorCount = 10;
        float yMaximum = 24f; // Maximum value for the y-axis (24 hours)
        float labelYIncrement = graphHeight / separatorCount;

        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelTransform = Instantiate(labelTemplateY);
            labelTransform.SetParent(graphContainer, false);
            labelTransform.gameObject.SetActive(true);
            float labelYPosition = i * labelYIncrement;
            labelTransform.anchoredPosition = new Vector2(-20f, labelYPosition);
            float labelValue = i * (yMaximum / separatorCount);
            labelTransform.GetComponent<Text>().text = labelValue.ToString("F0") + "h"; // Display time in hours
        }
    }

    private Dictionary<string, float> LoadAccumulatedTimes()
    {
        Dictionary<string, float> accumulatedTimes = new Dictionary<string, float>();
        string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

        foreach (string weekday in weekdays)
        {
            float accumulatedTime = PlayerPrefs.GetFloat(weekday, 0f);
            accumulatedTimes[weekday] = accumulatedTime;
        }

        return accumulatedTimes;
    }

    private string GetCurrentWeekday()
    {
        // Get the current system time and convert it to the corresponding weekday
        return System.DateTime.Now.ToString("ddd");
    }

}
