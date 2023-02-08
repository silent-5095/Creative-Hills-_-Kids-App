using System;
using UnityEngine;
using UnityEngine.Events;

public class Transition : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public event Action ExitAction;
    [SerializeField] private UnityEvent OnStartSceneEvent;
    public void Enter()
    {
        animator.Play("Transition_Enter");
    }

    private void EnterEventInvoker()
    {
        OnStartSceneEvent?.Invoke();
    }
    public void Exit(Action action)
    {
        ExitAction = action;
        animator.Play("Transition_Exit");
    }

    private void ExitEventInvoker()
    {
        ExitAction?.Invoke();
    }
}
