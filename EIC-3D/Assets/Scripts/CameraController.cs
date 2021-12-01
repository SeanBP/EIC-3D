using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    public bool useOffsetValues;

    public float rotateSpeed;

    public Transform pivot;
    private bool looping = false;
    public float maxViewAngle;
    public float minViewAngle;
    public PauseMenu pauseMenu;
    private bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
  
        if (!useOffsetValues)
        {
            offset = target.position - transform.position;
        }
        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        isPaused = pauseMenu.GameIsPaused;
        
            float sensitivity = 1f;

            string[] texts = Input.GetJoystickNames();
            for (int i = 0; i < texts.Length; i++)
            {
                if (!string.IsNullOrEmpty(texts[i]))
                {
                    sensitivity = 0.1f;
                }
            }
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed * sensitivity;
            float vertical = Input.GetAxis("Mouse Y") * rotateSpeed * sensitivity;
        if (!isPaused)
        {
            pivot.Rotate(-vertical, 0, 0);
            target.Rotate(0, horizontal, 0);
        }
        else
        {
            pivot.Rotate(0, 0, 0);
            target.Rotate(0, 0, 0);
        }

            if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f)
            {
                pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
            }
            if (pivot.rotation.eulerAngles.x > 180 && pivot.rotation.eulerAngles.x < 360f + minViewAngle)
            {
                pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0, 0);
            }

            float desiredYAngle = target.eulerAngles.y;
            float desiredXAngle = pivot.eulerAngles.x;

            Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
            transform.position = target.position - (rotation * offset);
            //transform.position = target.position - offset;

            if (transform.position.y < target.position.y - 0.5f)
            {
                transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
            }
            if (looping == true)
            {
                target.transform.position = new Vector3(0f, 0f, 0f);
            }
            transform.LookAt(target);
        
    }

    public void StopLooping()
    {
        looping = false;
    }

    public void Looping()
    {
        if(looping == false)
        {
            looping = true;
        }
        else
        {
            looping = false;
        }
    }
}
