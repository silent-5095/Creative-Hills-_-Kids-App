using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField][Range(1,50)]
    private float blinkingRange;
    private float _blinkingDelay,_blinkingTimer;

    private void Update()
    {
        _blinkingTimer += Time.fixedDeltaTime;
        if (_blinkingTimer >= _blinkingDelay)
        {
            Blinking();
        }
    }

    private void Blinking()
    {
        _blinkingTimer = 0;
        _blinkingDelay = Random.Range(1, blinkingRange);
        animator.SetTrigger(AnimationType.Blinking.ToString());
    }
}

public enum AnimationType
{
    Idle,
    Blinking,
    Run,
    JumpStart,
    JumpLoop,
    JumpEnd,
    Fall
}
