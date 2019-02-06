using UnityEngine;

public class Crow : StateMachineBehaviour
{
    float t = 0f;
    float tDrop;
    [SerializeField] float flyingVel;
    float tSpawnPU;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        GameManager.instance.CrowPos = animator.transform.localPosition;
        tSpawnPU = Random.Range(4f, 8f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        t += Time.deltaTime;
        tDrop += Time.deltaTime;
        animator.transform.parent.eulerAngles = new Vector3(0, t * flyingVel, 0);

        if (tDrop > tSpawnPU) {
            animator.SetBool("DropPU", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("DropPU", false);
        tDrop = 0f;
    }
}
