using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public float timeToPaint = 10;
    public bool timerToPaintIsRunning = false;
    public Text timeText;

    //public float timeToResetLastScreen = 5f;
    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (!UIManager.Instance.isLastScreen && UIManager.Instance.disableOnTouch.Count <= 0)
        {
            if (timerToPaintIsRunning)
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
                    timerToPaintIsRunning = false;
                    //StartCoroutine(UIManager.Instance.GoLastScreen());
                    StartCoroutine(AnimationManager.Instance.AnimateThirdToLast());
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

    public void FinisedPainting()
    {
        timeToPaint = 0;
        DisplayTime(-1);
    }
}