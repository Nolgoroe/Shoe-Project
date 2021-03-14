using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public float timeToPaint = 10;
    public bool timerIsRunning = false;
    public Text timeText;

    public float timeToReset = 5f;
    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeToPaint > 0)
            {
                timeToPaint -= Time.deltaTime;
                DisplayTime(timeToPaint);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeToPaint = 0;
                timerIsRunning = false;
                StartCoroutine(UIManager.Instance.GoLastScreen());
            }
        }

        if (UIManager.Instance.isLastScreen)
        {
            if (AnimationManager.Instance.isOutOfGame)
            {
                return;
            }
            else
            {
                if (timeToReset > 0)
                {
                    timeToReset -= Time.deltaTime;
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void TimerToReset()
    {

    }
}