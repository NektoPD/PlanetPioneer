using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CrateAnimator : MonoBehaviour
{
    private const string OpenTrigger = "Open";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ActivateOpenTrigger()
    {
        _animator.SetTrigger(OpenTrigger);
    }
}
