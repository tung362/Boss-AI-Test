using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 Direction = Vector3.zero;
    public bool CanRun = true;

    void Update()
    {
        UpdateRotation();
    }

    void UpdateRotation()
    {
        if(CanRun) transform.Rotate(Direction * Time.deltaTime);
    }

    public void Toggle(bool TrueFalse)
    {
        CanRun = TrueFalse;
    }
}
