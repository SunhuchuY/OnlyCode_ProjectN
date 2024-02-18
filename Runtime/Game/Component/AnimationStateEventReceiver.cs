using UnityEngine;

public class AnimationStateEventReceiver : StateMachineBehaviour
{
    public event System.Action<AnimatorStateInfo, int> OnStateEnterEvent;
    public event System.Action<AnimatorStateInfo, int> OnStateExitEvent;
    public event System.Action<AnimatorStateInfo, int> OnStateUpdateEvent;
    public event System.Action<AnimatorStateInfo, int> OnStateMoveEvent;
    public event System.Action<AnimatorStateInfo, int> OnStateIKEvent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        => OnStateEnterEvent?.Invoke(stateInfo, layerIndex);

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        => OnStateExitEvent?.Invoke(stateInfo, layerIndex);

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        => OnStateUpdateEvent?.Invoke(stateInfo, layerIndex);

    // OnStateMove is called right after Animator.OnAnimatorMove()
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        => OnStateMoveEvent?.Invoke(stateInfo, layerIndex);

    // OnStateIK is called right after Animator.OnAnimatorIK()
    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        => OnStateIKEvent?.Invoke(stateInfo, layerIndex);
}