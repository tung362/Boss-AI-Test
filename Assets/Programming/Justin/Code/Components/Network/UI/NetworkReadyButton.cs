using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkReadyButton : MonoBehaviour {

    Button button;

    // Use this for initialization
    void Awake()
    { button = GetComponent<Button>(); button.onClick.AddListener(OnClick); }

    // Update is called once per frame
    void OnClick()
    { Player.localPlayers[0].lobbyPlayer.readyToBegin = !Player.localPlayers[0].lobbyPlayer.readyToBegin; MatchManager.instance.CheckReadyToBegin(); }
}
