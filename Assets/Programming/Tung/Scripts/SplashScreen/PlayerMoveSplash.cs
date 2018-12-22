using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSplash : MonoBehaviour
{
    /*Settings*/
    public GameObject Target; //If there is a target ignores target position
    public Vector3 TargetPosition = Vector3.zero; //Only used when there is no targeted gameobject
    public float StopDistance = 0.5f;
    public float MoveSpeed = 2;
    public bool StartAnimating = false;

    /*Required components*/
    public Animator TheAnimator;

    void Update()
    {
        UpdateFollow();
    }

    void UpdateFollow()
    {
        if (!StartAnimating) return;

        Vector3 DesiredPosition = TargetPosition;
        if (Target != null) DesiredPosition = Target.transform.position;

        float distance = Vector3.Distance(transform.position, DesiredPosition);
        if (distance <= StopDistance) TheAnimator.SetBool("Run", false);
        else
        {
            TheAnimator.SetBool("Run", true);
            Vector3 direction = (DesiredPosition - transform.position).normalized;
            direction.y = 0;
            //Look
            transform.LookAt(new Vector3(DesiredPosition.x, transform.position.y, DesiredPosition.z));
            //Move
            transform.position += direction * MoveSpeed * Time.deltaTime;
        }
    }

    public void StartAnimation()
    {
        StartAnimating = true;
    }

    public void StopAnimation()
    {
        StartAnimating = false;
    }
}
