//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR;
//using UnityEngine.XR.Interaction.Toolkit;

//public class WhiteboardPen : XRDirectInteractor {

//	private InputDevice controllerActions;
//	private InputDevice controllerEvents;
//	public Whiteboard whiteboard;
//	private RaycastHit touch;
//	private Quaternion lastAngle;
//	private bool lastTouch;

//	// Use this for initialization
//	void Start ()
//    {
//		// Get our Whiteboard component from the whiteboard object
//		this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard>();
//	}

//	// Update is called once per frame
//	void Update ()
//    {
//		float tipHeight = transform.Find ("Tip").transform.localScale.y;
//		Vector3 tip = transform.Find ("Tip").transform.position;

//		Debug.Log (tip);

//		if (lastTouch) {
//			tipHeight *= 1.1f;
//		}

//		if(this.controllerEvents != null){
//			Debug.LogWarning("#Triggert");
//		}

//		// Check for a Raycast from the tip of the pen
//		if (Physics.Raycast (tip, transform.up, out touch, tipHeight) && this.controllerEvents != null)
//        {
//			if (!(touch.collider.tag == "Whiteboard")) return;

//    		whiteboard = touch.collider.GetComponent<Whiteboard>();

//			// Set whiteboard parameters
//			whiteboard.SetColor (Color.blue);
//			whiteboard.SetTouchPosition (touch.textureCoord.x, touch.textureCoord.y);
//			whiteboard.ToggleTouch (true);

//			// If we started touching, get the current angle of the pen
//			if (lastTouch == false)
//            {
//				lastTouch = true;
//				lastAngle = transform.rotation;
//			}
//		}
//        else
//        {
//			whiteboard.ToggleTouch (false);
//			lastTouch = false;
//		}

//		// Lock the rotation of the pen if "touching"
//		if (lastTouch)
//        {
//			transform.rotation = lastAngle;
//		}
//	}

//	public override void Grabbed(GameObject grabbingObject)
//	{
//		base.Grabbed(grabbingObject);
//		controllerActions = grabbingObject.GetComponent<VRTK_ControllerActions>();
//		controllerEvents = grabbingObject.GetComponent<LeftController>();
//	}

//    public override void 
//}
