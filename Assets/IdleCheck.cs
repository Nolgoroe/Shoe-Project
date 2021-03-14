using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleCheck : MonoBehaviour
{
    public static IdleCheck Instance;
    public float idleTime;
    public float timeTillReset = 10;

    private void Start()
    {
        Instance = this;
    }
    void Update()
    {
        if(Input.touchCount == 0)
        {
            idleTime += Time.deltaTime;

            if(idleTime > timeTillReset)
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            idleTime = 0;
        }
    }
}
