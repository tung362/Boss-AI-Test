using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class NetworkPortField : MonoBehaviour
{

    InputField input;

    void Awake()
    { input = GetComponent<InputField>(); input.onValueChanged.AddListener(UpdateInput); }

	void OnEnable ()
    { input.text = MatchManager.instance.networkPort.ToString(); }
	
	void UpdateInput (string value)
    { MatchManager.instance.networkPort = int.Parse(value); }
}
