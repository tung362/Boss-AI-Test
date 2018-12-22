using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Actor
{
    protected override void AnimationUpdate()
    {
        base.AnimationUpdate();
        animator.animator.SetBool("Moving", Mathf.Abs(rigidbody.velocity.magnitude) > 0.05f);
    }
}