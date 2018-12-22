using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerManager : MonoBehaviour
{
    //Inputs
    [HideInInspector]
    public float LEFTRIGHTVALUE;
    [HideInInspector]
    public float UPDOWNVALUE;
    [HideInInspector]
    public bool UP;
    [HideInInspector]
    public bool DOWN;
    [HideInInspector]
    public bool LEFT;
    [HideInInspector]
    public bool RIGHT;
    [HideInInspector]
    public bool DODGE;
    [HideInInspector]
    public bool BLOCK;
    [HideInInspector]
    public bool ATTACK;
    [HideInInspector]
    public bool THROW;
    [HideInInspector]
    public bool REACH;

    /*Data*/

    //Components this script will be managing
    private OldPlayerAnimator TheAnimator;
    private OldPlayerController TheController;

    void Start()
    {
        TheAnimator = GetComponent<OldPlayerAnimator>();
        TheController = GetComponent<OldPlayerController>();
    }

    void Update()
    {
        Inputs();
        TheAnimator.Animate();
        TheController.Control();
    }

    //void FixedUpdate()
    //{
    //    //TheAnimator.Fall();
    //}

    void LateUpdate()
    {

    }

    void Inputs()
    {
        LEFTRIGHTVALUE = Input.GetAxis("Horizontal");
        UPDOWNVALUE = Input.GetAxis("Vertical");
        UP = Input.GetAxis("Vertical") > 0;
        DOWN = Input.GetAxis("Vertical") < 0;
        LEFT = Input.GetAxis("Horizontal") < 0;
        RIGHT = Input.GetAxis("Horizontal") > 0;
        DODGE = Input.GetButtonDown("Fire2");
        ATTACK = Input.GetButtonDown("Fire1");
        //BLOCK = Input.GetButton("Block");
        //THROW = Input.GetButton("Throw");
        //REACH = Input.GetButton("Reach");
    }
}
