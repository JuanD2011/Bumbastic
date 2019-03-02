using UnityEngine;

public class MenuInitBehaviour : StateMachineBehaviour
{
    [SerializeField] Settings settings;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (settings.isNicknameSet)
        {
            animator.SetTrigger("NicknameSet");
        }
    }
}
