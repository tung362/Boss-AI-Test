using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class NetworkMatchNameField : MonoBehaviour {

    InputField input;

    void Awake()
    { input = GetComponent<InputField>(); input.onValueChanged.AddListener(UpdateInput); }

    void OnEnable()
    { input.text = MatchManager.instance.matchName; }

    void UpdateInput(string value)
    { MatchManager.instance.matchName = value; }
}
