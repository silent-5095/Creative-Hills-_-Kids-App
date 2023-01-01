using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;

namespace SlidingPipes
{
    public class SlidingBall : MonoBehaviour
    {
        [SerializeField] private Transform gfx;
        [SerializeField] private float speed;
        [SerializeField] private bool canRoll;
        private bool _isMoving;


        private void Update()
        {

            if (canRoll)
            {
                RollingAnimation();
            }
        }

        private void RollingAnimation()
        {
            gfx.Rotate(new Vector3(0, 0, Time.fixedDeltaTime * speed));
        }

        public void Move(Vector2 target)
        {
            transform.Translate(target);
        }

        
    }
}