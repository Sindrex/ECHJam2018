﻿namespace GameJam
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        /* Author: Sindrex
         * Date: 14.12.2018
         * 
         * Controls: 
         * -A/D, or right/left arrows to move around
         * -Space to jump
         * -Lshift to sprint/run
         * 
         */

        public float moveSpeed;
        public float sprintSpeed;
        private float currentMoveSpeed;
        public float jumpPower;
        public float jumpGroundDist;
        public float jumpRaycastStart;
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;
        public float maxFallVelocity = 2;

        public bool jumping = false;
        public bool running = false;
        public SpriteRenderer spriteRenderer;
        public GameState gameState;
        public Animator animator;
        private Rigidbody2D rb;
        public SoundController soundController;

	    // Use this for initialization
	    void Start () {
            rb = GetComponent<Rigidbody2D>();
            currentMoveSpeed = moveSpeed;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (jumping)
            {
                //print("jumping true");
                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, jumpRaycastStart, 0), Vector2.down, jumpGroundDist);
                if (hit)
                {
                    //print("false: " + hit.transform.gameObject.name);
                    if (!hit.transform.tag.Equals("Player"))
                    {
                        jumping = false;
                    }
                }
            }
            animator.SetBool("Jumping", jumping);

            // Ignore input if needed
            if (gameState.IgnoreInput) return;
            if (Input.GetKeyDown(KeyCode.Space) && !jumping)
            {
                //print("yo");
                GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpPower;
                jumping = true;
                soundController.playAudio("jump");
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                running = true;
                currentMoveSpeed = sprintSpeed;
            }
            else
            {
                running = false;
                currentMoveSpeed = moveSpeed;
            }
        }

        private void FixedUpdate()
        {
            FixSmallVelocity();
            //print(rb.velocity.y + "/" + -maxFallVelocity);
            if (rb.velocity.y < 0 && rb.velocity.y > -maxFallVelocity)
            {
                //print("B");
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && rb.velocity.y > -maxFallVelocity)
            {
                //print("A");
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            else if(rb.velocity.y < -maxFallVelocity)
            {
                rb.velocity = new Vector2(rb.velocity.x, -maxFallVelocity);
            }

            // Ignore input if needed
            float horizontalAxis = (gameState.IgnoreInput) ? 0f : Input.GetAxis("Horizontal");
            animator.SetFloat("VerticalSpeed", rb.velocity.y);
            animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalAxis * currentMoveSpeed * Time.deltaTime));

            // Ignore input if needed
            if (gameState.IgnoreInput) return;
            if (horizontalAxis != 0f) spriteRenderer.flipX = (horizontalAxis < 0);

            transform.Translate(horizontalAxis * currentMoveSpeed * Time.deltaTime, 0, 0);
            //rb.velocity += Vector2.right * horizontalAxis * currentMoveSpeed * Time.deltaTime * 5;
            //rb.AddForce(Vector2.right * horizontalAxis * currentMoveSpeed *7);
        }

        void FixSmallVelocity()
        {
            if (Mathf.Abs(rb.velocity.y) < 0.0001f)
            {
                var v = rb.velocity;
                v.y = 0f;
                rb.velocity = v;
            }
        }
    }
}