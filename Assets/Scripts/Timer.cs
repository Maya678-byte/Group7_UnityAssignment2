using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timeElapsed = 0f; // starts at 0
    private Text timerText;
    private bool timerIsRunning = true;

    void Start()
    {
        timerText = GetComponent<Text>();
    }

    void Update()
    {
        if (ValueSingleton.Instance.health <= 0)
        {
            timerIsRunning = false;
        }
        if (timerIsRunning)
        {
            timeElapsed += Time.deltaTime; // count up
            DisplayTime(timeElapsed);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        ValueSingleton.Instance.timerText = timerText.text;
    }
}