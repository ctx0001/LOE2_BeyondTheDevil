using UnityEngine;

namespace GameScene.Player
{
    public class Gravity : MonoBehaviour
    {
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private CharacterController controller;

        private const float GravityForce = -0.8f;
        private const float GroundCheckRadius = 0.01f;

        private Vector3 _speed;
        private bool _isGrounded;
        
        private float _smoothSpeed;
        private float _smoothTime = 0.2f;

        private void FixedUpdate()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.transform.position, GroundCheckRadius, groundMask);
            // Apply gravity if not grounded
            if (!_isGrounded)
            {
                _speed.y += GravityForce * Time.fixedDeltaTime;
                _speed.y = Mathf.SmoothDamp(_speed.y, _speed.y + GravityForce * Time.fixedDeltaTime, ref _smoothSpeed, _smoothTime);
            }
            controller.Move(_speed * Time.fixedDeltaTime);
        }
    }
}