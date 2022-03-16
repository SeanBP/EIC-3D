using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class VerticalMovement : MonoBehaviour
{
    public GameObject player;
    public CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        XRSettings.eyeTextureResolutionScale = 2f;
    }

    // Update is called once per frame
    void Update()
    {

        float vertMag = 0f;

        var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
        foreach (var device in rightHandDevices)
        {       
            Vector2 joyPos;
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joyPos))
            {
                vertMag = joyPos.y;
            }
        }


        controller.Move(new Vector3(0f, vertMag * Time.deltaTime, 0f));
    
    }
}
