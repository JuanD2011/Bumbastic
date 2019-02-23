using UnityEngine;

public class FlyUp : StateMachineBehaviour
{
    [SerializeField] float timeToUp;
    float distance;
    Vector3 target;
    [SerializeField] InGame inGame;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        target.x = (inGame.CrowPos.x > 0) ? inGame.CrowPos.x * -1 : inGame.CrowPos.x;
        target = new Vector3(target.x, inGame.CrowPos.y, inGame.CrowPos.z);
        distance = Vector3.Distance(animator.transform.position, target);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector3.MoveTowards(animator.transform.position, target, (Time.deltaTime * distance) / timeToUp);
        if (animator.transform.position == target)
        {
            animator.SetBool("Flying",true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("Flying", false);
    }
}
