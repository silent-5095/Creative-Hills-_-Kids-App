using System;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public event Action ExitAction;
    public void Enter()
    {
        animator.Play("Transition_Enter");
    } public void Exit(Action action)
    {
        ExitAction = action;
        animator.Play("Transition_Exit");
    }

    public void ExitAnimation()
    {
        ExitAction?.Invoke();
    }
}
