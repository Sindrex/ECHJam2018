using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    /* Author: Sindrex
     * Date: 14.12.2018
     * 
     * Controls: 
     * -A/D, or right/left arrows to move around
     * -Space to jump
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

    public bool jumping = false;
    public bool running = false;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        currentMoveSpeed = moveSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (jumping)
        {
            //print("jumping true");
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, jumpRaycastStart, 0), Vector2.down, jumpGroundDist);
            if(hit)
            {
                //print("false: " + hit.transform.gameObject.name);
                if (!hit.transform.tag.Equals("Player"))
                {
                    jumping = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            //print("yo");
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpPower;
            jumping = true;
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
        transform.Translate(Input.GetAxis("Horizontal") * currentMoveSpeed * Time.deltaTime, 0, 0);

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
