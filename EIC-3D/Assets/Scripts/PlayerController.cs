using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveDirection;
    public CharacterController controller;
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
}
