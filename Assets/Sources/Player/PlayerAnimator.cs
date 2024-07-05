using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private const string RunningAnimationState = "IsRunning";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetRunningAnimation(bool value)
    {
        _animator.SetBool(RunningAnimationState, value);
    }
}
