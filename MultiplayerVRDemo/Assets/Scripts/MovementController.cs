using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementController : MonoBehaviour
{
    public float playerSpeed = 1.0f;
    public List<XRController> Controllers;
    public GameObject playerHead = null;

    [SerializeField]
    TeleportationProvider teleportationProvider;

    public GameObject mainVRPlayer;
    public GameObject XRRigGameobject;

    //Start is called before the first frame update.
    void Start()
    {

    }

    private void OnEnable()
    {
        teleportationProvider.endLocomotion += onEndLocomotion;
    }

    private void OnDisable()
    {
        teleportationProvider.endLocomotion -= onEndLocomotion;
    }

    void onEndLocomotion(LocomotionSystem locomotionSystem)
    {
        //Be notified when the teleportation is now ended.
        Debug.Log("Teleportation ended.");
        mainVRPlayer.transform.position = mainVRPlayer.transform.TransformPoint(XRRigGameobject.transform.localPosition);
        XRRigGameobject.transform.localPosition = Vector3.zero;
    }

    //Update is called once per frame.
    void Update()
    {
        //For the controllers in the Controllers List.
        foreach (XRController xRController in Controllers)
        {
            //Get the input from the controller's primary 2D axis (joystick most likely).
            if (xRController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 positionVector))
            {
                //If there is a change in the joystick vector.
                if (positionVector.magnitude > 0.15f)
                {
                    //Move the player in that vector by that amount.
                    movePlayer(positionVector);
                }
            }
        }
    }

    private void movePlayer(Vector2 positionVector)
    {
        //Apply the touch position to the head's forward vector.
        Vector3 direction = new Vector3(positionVector.x, 0, positionVector.y);
        Vector3 headRotation = new Vector3(0, playerHead.transform.eulerAngles.y, 0);

        //Rotate the input direction by the horizontal head rotation.
        direction = Quaternion.Euler(headRotation) * direction;

        //Apply speed and move.
        Vector3 movement = direction * playerSpeed;
        transform.position += (Vector3.ProjectOnPlane(Time.deltaTime * movement, Vector3.up));
    }
}
