using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLaserSkullSpinner : MonoBehaviour
{
    /*Settings*/
    public Vector3 RotateDirection = Vector3.zero;
    public bool HeightLoop = false;
    public Vector3 UpLimit = Vector3.zero;
    public Vector3 DownLimit = Vector3.zero;
    public bool GoUp = true;
    public float UpDownSpeed = 0.5f;
    public float CurrentValue = 0;

    /*Data*/
    private bool Spin = true;

    void Update ()
    {
        UpdateSpinner();
    }

    void UpdateSpinner()
    {
        if (!Spin) return;

        transform.Rotate(RotateDirection * Time.deltaTime);

        if(HeightLoop)
        {
            if (GoUp)
            {
                CurrentValue += UpDownSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(DownLimit, UpLimit, CurrentValue);
                if (CurrentValue >= 1) GoUp = false;
            }
            else
            {
                CurrentValue -= UpDownSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(DownLimit, UpLimit, CurrentValue);
                if (CurrentValue <= 0) GoUp = true;
            }
            CurrentValue = Mathf.Clamp(CurrentValue, 0, 1);
        }
    }

    public void Toggle(bool TrueFalse)
    {
        Spin = TrueFalse;
    }
}
