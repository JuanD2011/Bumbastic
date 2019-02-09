using UnityEngine;

public class DropPU : StateMachineBehaviour
{
    [SerializeField] float timeToDrop;
    float distance;
    Vector3 center = new Vector3(0f, -9.95f, 0f);
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        distance = Vector3.Distance(animator.transform.localPosition, center);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.localPosition = Vector3.MoveTowards(animator.transform.localPosition, center, (Time.deltaTime * distance)/timeToDrop);
        if (animator.transform.localPosition == center)
        {
            if (animator.transform.childCount > 1) {
                animator.transform.GetChild(1).parent = null;
            }
            animator.SetBool("PUDropped",true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("PUDropped", false);
    }
}
