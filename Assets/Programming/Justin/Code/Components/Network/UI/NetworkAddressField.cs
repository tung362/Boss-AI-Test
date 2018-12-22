using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class NetworkAddressField : MonoBehaviour {

    InputField input;

    void Awake()
    { input = GetComponent<InputField>(); input.onValueChanged.AddListener(UpdateInput); }

    void OnEnable()
    { input.text = MatchManager.instance.networkAddress; }

    void UpdateInput(string value)
    { MatchManager.instance.networkAddress = value; }
}
