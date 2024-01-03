using System;
using Unity.FPS.Gameplay;
using UnityEngine;

namespace FPS.Character.Scripts
{
    public class PlayerAnim : MonoBehaviour
    {
        [Tooltip("The behaviour responsible for movements.")]
        public PlayerCharacterController _playerCharacterController;
        
        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        
        private Animator _animator;
        
        private bool _hasAnimator;

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);

            AssignAnimationIDs();
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, _playerCharacterController.IsGrounded);
            }
        }

        private void Move()
        {
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, Math.Abs(_playerCharacterController.CharacterVelocity.x) + Math.Abs(_playerCharacterController.CharacterVelocity.z));
                _animator.SetFloat(_animIDMotionSpeed, Math.Min(_playerCharacterController.CharacterVelocity.magnitude, 1f));
            }
        }

        private void JumpAndGravity()
        {
            if (_playerCharacterController.IsGrounded)
            {
                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // Jump
                if (_playerCharacterController.HasJumpedThisFrame)
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }
            }
            else
            {
                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }
        }
    }
}