using Networking.GameServer;
using UnityEngine;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float speed = 15.0f, jumpForce = 20.0f;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer mRenderer;
        [SerializeField] private Rigidbody2D body;
        
        private float _movementX;
        private bool IsGrounded { get; set; } = true;
        
        private void Update()
        {
            _movementX = Input.GetAxis("Horizontal");
            transform.Translate(_movementX * (speed * Time.deltaTime), 0.0f, 0.0f);
            Animate();
            Jump();
            if (_movementX > 0 || !IsGrounded)
                GameServerSend.PlayerMove(transform.position);
        }

        private void Jump()
        {
            if (!Input.GetButtonDown("Jump") || !IsGrounded) return;
            IsGrounded = false;
            body.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
                IsGrounded = true;
        }

        private void Animate()
        {
            switch (_movementX)
            {
                case > 0:
                    mRenderer.flipX = false;
                    animator.SetBool(Global.WALK_ANIM, true);
                    break;
                case < 0:
                    mRenderer.flipX = true;   
                    animator.SetBool(Global.WALK_ANIM, true);
                    break;
                default:
                    animator.SetBool(Global.WALK_ANIM, false);
                    break;
            }
        }
    }
}
