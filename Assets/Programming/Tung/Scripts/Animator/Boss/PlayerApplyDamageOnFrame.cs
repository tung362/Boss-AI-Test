using System.Collections;
using UnityEngine;

public class PlayerApplyDamageOnFrame : StateMachineBehaviour
{
    /*Settings*/
    public float TriggeredAtFrame = 0;
    public float Speed = 1000;
    public string LimitVariableName = "";

    /*Data*/
    private bool RunOnce = true;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RunOnce = true;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= TriggeredAtFrame && RunOnce)
        {
            RunOnce = false;
        }
    }
}
