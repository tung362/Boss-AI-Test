using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NetworkDisconnectButton : MonoBehaviour {

    Button button;

    // Use this for initialization
    void Awake()
    { button = GetComponent<Button>(); button.onClick.AddListener(OnClick); }

    // Update is called once per frame
    void OnClick()
    { MatchManager.instance.Disconnect(); }
}
