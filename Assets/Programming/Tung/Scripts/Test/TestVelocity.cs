using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVelocity : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GetComponent<Rigidbody>().velocity = transform.forward * 1000 * Time.deltaTime;
            Debug.Log("GO");
        }

    }
}
