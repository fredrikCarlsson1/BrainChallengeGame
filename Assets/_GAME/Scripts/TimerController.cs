using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TimerController : MonoBehaviour {

    Image timebar;
    private float maxTime = 85;
    float timeLeft;


	void Start () {
        timebar = GetComponent<Image>();
        timeLeft = maxTime;
        Debug.Log(maxTime);
    }

	
	// Update is called once per frame
	void Update () {
        if (timeLeft < 30 && timeLeft > 15)
            timebar.color = Color.yellow;

        else if (timeLeft <= 15)
        {
            timebar.color = Color.red;
        }

        else
            timebar.color = Color.green;


        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;

            timebar.fillAmount = timeLeft / maxTime;
        }
        else {
            GameManager.Instance.GameOver();
        }
		
	}

    public void RestartTimerAndReduceTime() {
        if (maxTime > 80) {
            maxTime = 70;
        }
        else if (maxTime > 19) {
            maxTime -= 5;
        }
        Debug.Log(maxTime);

        timeLeft = maxTime;
    }
}
