using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip slideAudio;
        public AudioClip runAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public AudioClip powerupAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed;
        /// <summary>
        /// Speed modifier while sliding as a multiplicative factor of max speed.
        /// </summary>
        public float slideSpeedModifier;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public HealthBar healthBar;
        public bool controlEnabled = true;

        bool jump;
        bool slide;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            healthBar.SetMaxHealth(health.maxHP);
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                // for now just move player to the right continually
                // TODO: it might be more efficient to keep player and camera stationary and move other things instead
                move.x = 1;//Input.GetAxis("Horizontal");
                var pressUpDown = Input.GetAxis("Vertical");

                if (jumpState == JumpState.Grounded && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(runAudio);
                }

                if (jumpState == JumpState.Grounded && pressUpDown > 0)
                    jumpState = JumpState.PrepareToJump;
                else if (pressUpDown <= 0)
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                slide = pressUpDown < 0;
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            healthBar.SetHealth(health.GetHP());
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
                case JumpState.Grounded:
                    if (slide)
                    {
                        Schedule<PlayerSlid>().player = this;
                        jumpState = JumpState.Sliding;
                    }
                    break;
                case JumpState.Sliding:
                    if (!slide)
                        jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            var isSliding = jumpState == JumpState.Sliding;
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            animator.SetBool("slide", isSliding);

            if (isSliding)
                targetVelocity = move * maxSpeed * slideSpeedModifier;
            else
                targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed,
            Sliding
        }
    }
}