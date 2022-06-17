using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Text timeText;

    [SerializeField]
    private GameObject timerOnButton;

    [SerializeField]
    private GameObject timerOffButton;

    public float TimeRemainingInSeconds { get; private set; }

    private bool isRunning = true;

    private readonly float heightUI = Screen.height * 0.05f;

    private readonly float widthUI = Screen.width * 0.25f;

    private void Start()
    {
        timerOnButton.SetActive(false);
        timerOffButton.SetActive(false);
        MoveUI();
        SizeUI();
    }

    private void Update()
    {
        if (isRunning)
        {
            if (TimeRemainingInSeconds > 0)
            {
                TimeRemainingInSeconds -= Time.deltaTime;
                DisplayTime(TimeRemainingInSeconds);
            }
            else
            {
                TimeRemainingInSeconds = 0;
                isRunning = false;
            }
        }
    }
    
    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00} \n Push to turn Off", minutes, seconds);
    }
    private void MoveUI()
    {
        transform.position = new Vector2(Screen.width * 0.25f, Screen.height * 0.90f);
        timerOnButton.transform.position = new Vector2(Screen.width * 0.25f, Screen.height * 0.90f);
    }

    private void SizeUI()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
        timerOnButton.GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void StartTimer(int value)
    {
        timerOffButton.SetActive(true);
        isRunning = true;
        if (value == 10)
        {
            TimeRemainingInSeconds = 240;
        }
        else if (value == 30)
        {
            TimeRemainingInSeconds = 300;
        }
        else
        {
            TimeRemainingInSeconds = 360;
        }
    }

    public void TimerOn(int value)
    {
        StartTimer(value);
        timerOnButton.SetActive(false);
        timerOffButton.SetActive(true);
    }

    public void TimerOff()
    {
        StopTimer();
        timerOnButton.SetActive(true);
        timerOffButton.SetActive(false);
    }
}