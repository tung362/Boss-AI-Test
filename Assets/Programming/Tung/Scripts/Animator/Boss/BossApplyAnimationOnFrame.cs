using UnityEngine;
using System.Collections;

public class BossApplyAnimationOnFrame : StateMachineBehaviour
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
            animator.transform.root.GetComponent<Rigidbody>().AddForce(animator.transform.root.forward * Speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
            animator.transform.root.GetComponent<TestBossAI>().GetType().GetField(LimitVariableName).SetValue(animator.transform.root.GetComponent<TestBossAI>(), true);
            RunOnce = false;
        }
    }
}
