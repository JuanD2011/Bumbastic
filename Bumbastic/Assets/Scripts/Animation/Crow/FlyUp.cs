using UnityEngine;

public class FlyUp : StateMachineBehaviour
{
    [SerializeField] float timeToUp;
    float distance;
    Vector3 target;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        target = new Vector3(-GameManager.instance.CrowPos.x, GameManager.instance.CrowPos.y, 0f);
        distance = Vector3.Distance(animator.transform.position, target);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.localPosition = Vector3.MoveTowards(animator.transform.localPosition, target, (Time.deltaTime * distance) / timeToUp);
        if (animator.transform.localPosition == target)
        {
            animator.SetBool("Flying",true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("Flying", false);
    }
}
