using UnityEngine;

namespace Platformer
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidBody;
        [SerializeField] private float speed,jumpSpeed,gravityFall;
        private bool _isJumped;
        private float _direction = 0;

        private void OnCollisionEnter2D(Collision2D other)
        {
            _isJumped = false;
            rigidBody.gravityScale = 1;
        }

        private void Look()
        {
            var tempScale = transform.localScale;
            if (_direction == 0) return;
            if (Mathf.Sign(_direction) * 1 > 0)
            {
                if (tempScale.x < 0)
                    tempScale.x *= -1;
            }
            else
            {
                if (tempScale.x > 0)
                    tempScale.x *= -1;
            }
            transform.localScale = tempScale;
        }

        private void Move()
        {
            var tempVelocity = rigidBody.velocity;
            tempVelocity.x = _direction * speed * Time.fixedDeltaTime;
            rigidBody.velocity = tempVelocity;
        }

        private void Jump()
        {
            if (rigidBody.velocity.y <= 0 && _isJumped)
                rigidBody.gravityScale = gravityFall;
            if (!Input.GetButton("Jump") || _isJumped) return;
            var tempVelocity = rigidBody.velocity;
            tempVelocity.y = jumpSpeed * Time.fixedDeltaTime;
            rigidBody.velocity = tempVelocity;
            _isJumped = true;

        }

        private void Update()
        {
            _direction = Input.GetAxis("Horizontal");
            Look();
        }

        private void FixedUpdate()
        {
            Jump();
            Move();
        }
    }
}