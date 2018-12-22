using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

//Disable gameobject when connected to a server
public class DisableOnConnect : MonoBehaviour
{
    /*Callable functions*/
    public UnityEvent OnToggle;

    /*Required components*/
    private NetworkManager TheNetworkManager;

    void Start ()
    {
        TheNetworkManager = FindObjectOfType<NetworkManager>();
    }
	
	void Update ()
    {
        if (TheNetworkManager.IsClientConnected())
        {
            OnToggle.Invoke();
            gameObject.SetActive(false);
        }
    }
}
