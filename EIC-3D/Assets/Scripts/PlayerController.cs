using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveDirection;
    private bool looping = false;
    public CharacterController controller;
    public GameObject player;
    public float jumpForce;
    public float gravityScale;
    
    // Start is called before the first frame update
    void Start()
    {
        //theRB = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (looping == false)
        {
            
             moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal")) + (transform.up * Input.GetAxis("Jump") * 0.5f);

             if (moveDirection.magnitude > 1)
             {
                 moveDirection = moveDirection.normalized * moveSpeed;
             }
             else
             {
                 moveDirection = moveDirection * moveSpeed;
             }

             controller.Move(moveDirection * Time.deltaTime);
            
        }
        else
        {
            player.transform.position = new Vector3(9*(float)Math.Cos(0.3f * Time.time) , 3f, 9 * (float)Math.Sin(0.3f * Time.time));
        }
    }
    public void StopLooping()
    {
        looping = false;
    }
    public void Looping()
    {
        if (looping == false)
        {
            looping = true;
        }
        else
        {
            looping = false;
        }
    }
}
