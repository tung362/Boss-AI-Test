using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OldPlayerInput : MonoBehaviour
{

    private static OldPlayerInput _localInstance;
    public static OldPlayerInput localInstance
    { get { return _localInstance; } }

    public static List<OldPlayerInput> players = new List<OldPlayerInput>();

    public LayerMask mask;

    public OldController controller;

    Vector3 movementInput
    { get { return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); } }

    public bool ready;

    private void OnEnable()
    {
        players.Add(this);
    }

    private void OnDisable()
    {
        players.Remove(this);
    }

    private void Start()
    {
        if (localInstance == null) _localInstance = this;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
        if (movementInput != null) MoveCommand();
        if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetButtonDown("Fire1")) AttackCommand();
            if (Input.GetButtonDown("Fire2")) DashCommand();
            if ((controller.stats.attacking || controller.stats.dashing)) AimCommand();
        }
	}

    void MoveCommand()
    { controller.RequestMovement(movementInput); }
    
    void AttackCommand()
    { controller.RequestAttack(); }

    void DashCommand()
    { controller.RequestDash(); }

    void AimCommand()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            controller.RequestRotation(Vector3.Normalize(hit.point - transform.position));
    }

    public void ReadyToggle()
    {
        CmdReadyToggle(!ready);
    }

    void CmdReadyToggle(bool r)
    {
        ready = r;
        RpcReadyToggle(r);
    }

    void RpcReadyToggle(bool r)
    {
        ready = r;
    }
}
