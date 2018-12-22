using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerAnimator : MonoBehaviour
{
    /*Required Components*/
    private OldPlayerManager TheManager;
    public Animator TheAnimator;

    void Start()
    {
        TheManager = GetComponent<OldPlayerManager>();
    }

    public void Animate()
    {
        Walk();
        Dash();
        Attack();
    }

    void Walk()
    {
        bool Walk = false;
        if (TheManager.UP || TheManager.DOWN || TheManager.LEFT || TheManager.RIGHT) Walk = true;
        TheAnimator.SetBool("Walk", Walk);
    }

    void Dash()
    {
        if (TheManager.DODGE) TheAnimator.SetBool("Dash", true);
    }

    void Attack()
    {
        if (TheManager.ATTACK) TheAnimator.SetBool("Attack", true);
    }
}
