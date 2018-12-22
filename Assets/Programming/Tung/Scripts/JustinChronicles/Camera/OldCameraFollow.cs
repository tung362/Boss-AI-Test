using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCameraFollow : MonoBehaviour
{

    private static OldCameraFollow _cameraFollow;
    public static OldCameraFollow cameraFollow
    { get { return _cameraFollow; } }

    public Transform followTarget;

    public Vector3 desiredOffset;
    public float moveSpeed;
    public float rotSpeed;

    private void Awake()
    {
        if (cameraFollow == null) _cameraFollow = this;
        else DestroyImmediate(this);
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
		if(followTarget != null)
        {
            transform.position = Vector3.Slerp(transform.position, followTarget.position + desiredOffset, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.Normalize(followTarget.position - transform.position)), rotSpeed * Time.deltaTime);
        }
	}

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }
}
