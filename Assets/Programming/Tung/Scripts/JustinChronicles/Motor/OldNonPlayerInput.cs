using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OldNonPlayerInput : NetworkBehaviour {

    public float attackRange;
    public OldController controller;

    Transform closestPlayer
    { get {
            Transform t = null;
            float d = Mathf.Infinity;
            foreach(OldPlayerInput p in OldPlayerInput.players)
            {
                float pd = Vector3.Distance(transform.position, p.transform.position);
                if (pd < d)
                {
                    d = pd;
                    t = p.transform;
                }
            }
            return t;
    }     }

    float distanceFromPlayer
    { get { return Vector3.Distance(transform.position, closestPlayer.position); } }

    Vector3 directionToPlayer
    { get { return Vector3.Normalize(closestPlayer.position - transform.position); } }

	// Use this for initialization
	void Start ()
    {
        if (!isServer) enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveCommand();
        AttackCommand();
        AimCommand();
	}

    void MoveCommand()
    {
        controller.RequestMovement((closestPlayer != null && distanceFromPlayer > attackRange) ? directionToPlayer : Vector3.zero);
    }

    void AttackCommand()
    {
        if (distanceFromPlayer <= attackRange) controller.RequestAttack();
    }

    void AimCommand()
    {
        if (closestPlayer != null) controller.RequestRotation(directionToPlayer);
    }
}
