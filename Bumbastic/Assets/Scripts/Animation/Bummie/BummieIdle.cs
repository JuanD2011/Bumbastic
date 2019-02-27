using UnityEngine;

public class BummieIdle : StateMachineBehaviour
{
    float tToBreak;
    float elapsedTime;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        tToBreak = Random.Range(20f, 25f);
        elapsedTime = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > tToBreak)
        {
            animator.SetBool("BreakIdle",true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("BreakIdle", false);
    }
}
