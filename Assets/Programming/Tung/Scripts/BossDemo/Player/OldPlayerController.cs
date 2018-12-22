using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerController : MonoBehaviour
{
    /*Settings*/
    public float DashDuration = 0.5f;
    public float DashCooldownDuration = 0.5f;
    public GameObject HitBox;

    /*Required Components*/
    private OldPlayerManager TheManager;
    private Rigidbody TheRigidbody;
    public Animator TheAnimator;
    private OldMotor TheMotor;

    /*Data*/
    private Vector3 AimPoint = Vector3.zero;
    [HideInInspector]
    public bool LimitDash = false;
    private float LimitDashTimer = 0;

    void Start()
    {
        TheManager = GetComponent<OldPlayerManager>();
        TheRigidbody = GetComponent<Rigidbody>();
        TheMotor = GetComponent<OldMotor>();
    }

    public void Control()
    {
        Walk();
        Dash();
        Attack();
    }

    void Walk()
    {
        //Move
        Vector3 theInput = new Vector3(TheManager.LEFTRIGHTVALUE, 0, TheManager.UPDOWNVALUE);
        Quaternion camRot = Quaternion.Euler(0, OldCameraFollow.cameraFollow.transform.rotation.eulerAngles.y, 0);
        if (theInput != Vector3.zero) TheMotor.Move(camRot * theInput, ForceMode.VelocityChange, false);

        //Rotation
        Vector3 direction = camRot * theInput;
        //direction.y = 0;
        if (direction != Vector3.zero) TheMotor.Rotate(Quaternion.LookRotation(direction.normalized));
    }

    void Dash()
    {
        if(TheManager.DODGE) Aim();

        if(TheAnimator.GetBool("Dash"))
        {
            Vector3 direction = (new Vector3(AimPoint.x, transform.position.y, AimPoint.z) - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10 * Time.fixedDeltaTime);
        }

        if(LimitDash)
        {
            LimitDashTimer += Time.deltaTime;
            if (LimitDashTimer >= 0.4f)
            {
                TheRigidbody.velocity = Vector3.zero;
                LimitDashTimer = 0;
                LimitDash = false;
            }
        }
    }

    void Attack()
    {
        if (TheManager.ATTACK) Aim();
        if (TheAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack1"))
        {
            Vector3 direction = (new Vector3(AimPoint.x, transform.position.y, AimPoint.z) - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 20 * Time.fixedDeltaTime);
            HitBox.SetActive(true);
        }
        else HitBox.SetActive(false);
    }

    //Tools///////////////////////////////////////////////////////////////////////
    public void Aim()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] mouseHits = Physics.RaycastAll(mouseRay);

        for (int i = 0; i < mouseHits.Length; i++)
        {
            if (mouseHits[i].transform.tag == "Ground")
            {
                AimPoint = mouseHits[i].point;
                break;
            }
        }
    }
}
