using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private const string RunningAnimationState = "IsRunning";
    private const string JumpAnimationTriggerName = "Jump";

    private readonly int _jumpTrigger = Animator.StringToHash(JumpAnimationTriggerName);
    private readonly int _runTrigger = Animator.StringToHash(RunningAnimationState);

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetRunningAnimation(bool value)
    {
        _animator.SetBool(_runTrigger, value);
    }

    public void SetJumpingAnimation()
    {
        _animator.SetTrigger(_jumpTrigger);
    }
}