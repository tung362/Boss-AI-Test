using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossPillar : MonoBehaviour
{
    /*Setting*/
    public float TargetHeight = 3.04f;
    public float Speed = 0.5f;
    public float CurrentValue = 0;
    //public float BreakSpeed = 1;

    /*Data*/
    private Vector3 StartingPosition = Vector3.zero;
    public bool Rise = false;
    public bool PlayerIsTouching = false;
    public bool Broken = false;

    void Start()
    {
        StartingPosition = transform.localPosition;
    }

    void Update()
    {
        UpdatePillar();
    }

    void UpdatePillar()
    {
        if (PlayerIsTouching) return;

        if(Broken || PlayerIsTouching) CurrentValue -= 3 * Time.deltaTime;
        else
        {
            if (Rise) CurrentValue += Speed * Time.deltaTime;
            else CurrentValue -= Speed * Time.deltaTime;
        }
        CurrentValue = Mathf.Clamp(CurrentValue, 0, 1);

        transform.localPosition = Vector3.Lerp(StartingPosition, new Vector3(StartingPosition.x, TargetHeight, StartingPosition.z), CurrentValue);
    }

    public void Toggle(bool TrueFalse)
    {
        Rise = TrueFalse;
    }

    public void Broke()
    {
        Broken = true;
        Rise = false;
    }

    void OnTriggerStay(Collider other)
    {
        if(other.transform.gameObject.layer == LayerMask.NameToLayer("Player") || other.transform.gameObject.layer == LayerMask.NameToLayer("Mob")) PlayerIsTouching = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player") || other.transform.gameObject.layer == LayerMask.NameToLayer("Mob")) PlayerIsTouching = false;
    }
}
