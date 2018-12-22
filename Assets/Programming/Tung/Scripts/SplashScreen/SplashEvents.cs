using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SplashEvents : MonoBehaviour
{
    /*Settings*/
    public GameObject Target; //The object that it is watching
    public GameObject[] Triggers; //Make sure it has the same size as OnToggle

    /*Callable functions*/
    public UnityEvent[] OnToggle; //Make sure it has the same size as Triggers
    private List<bool> AlreadyCalledIds = new List<bool>();

    void Update()
    {
        UpdateTriggerEvents();
        for (int i = 0; i < Triggers.Length; ++i) AlreadyCalledIds.Add(false);
    }

    void UpdateTriggerEvents()
    {
        for(int i = 0; i < Triggers.Length; ++i)
        {
            if (Target.transform.position.x > Triggers[i].transform.position.x && !AlreadyCalledIds[i] && OnToggle[i] != null)
            {
                AlreadyCalledIds[i] = true;
                OnToggle[i].Invoke();
            }
        }
    }
}
