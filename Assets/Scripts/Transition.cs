using System;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Action exitAction;
    public void Enter()
    {
        animator.Play("Transition_Enter");
    } public void Exit(Action action)
    {
        exitAction = action;
        animator.Play("Transition_Exit");
    }

    public void ExitAnimation()
    {
        exitAction?.Invoke();
    }
}
