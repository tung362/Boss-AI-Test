using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

//Go to new scene after a certain amount of time has passed or toggle something
public class GoToLevel : MonoBehaviour
{
    /*Settings*/
    public string levelName = "";
    public float Delay = 5;
    public bool InstantTransition = true;
    public bool StartCountdown = true;

    /*Data*/
    private float Timer = 0;

    /*Callable Function*/
    public UnityEvent OnToggle;

	void Update ()
    {
        UpdateTimer();
	}

    void UpdateTimer()
    {
        if(!StartCountdown) return;
        Timer += Time.deltaTime;
        if (Timer >= Delay)
        {
            if(InstantTransition) SceneManager.LoadScene(levelName);
            if(OnToggle != null) OnToggle.Invoke();
            Timer = 0;
        }
    }

    public void ToggleCountdown()
    {
        StartCountdown = !StartCountdown;
    }
}
