using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuickIntroSkip : MonoBehaviour
{
    public UnityEvent OnToggle;

    void Update()
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) OnToggle.Invoke();
    }
}
