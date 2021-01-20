using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class HandButton : XRBaseInteractable
{
    public GameObject Player;
    private SphereCollider sphere;

    public UnityEvent OnPress = null;
    public UnityEvent OnDepress = null;

    private float yMin = 0.0f;
    private float yMax = 0.0f;
    private bool previousPress = false;

    private float previousHandHeight = 0.0f;
    private XRBaseInteractor hoverInteractor = null;

    //Add in hover events to allow the button to detect the collision from the player.
    protected override void Awake()
    {
        base.Awake();
        onHoverEnter.AddListener(StartPress);
        onHoverExit.AddListener(EndPress);
    }

    //Remove listeners for clean-up.
    protected void OnDestroy()
    {
        onHoverEnter.RemoveListener(StartPress);
        onHoverExit.RemoveListener(EndPress);
    }

    //When the press begins; assign the interactor.
    private void StartPress(XRBaseInteractor interactor)
    {
        hoverInteractor = interactor;

        //Allows the button to work at any orientation.
        previousHandHeight = GetLocalYPosition(hoverInteractor.transform.position);
    }

    //When the press ends; unassign the interactor.
    private void EndPress(XRBaseInteractor interactor)
    {
        hoverInteractor = null;
        previousHandHeight = 0.0f;

        previousPress = false;
        SetYPosition(yMax);
    }

    private void Start()
    {
        sphere = Player.GetComponentInChildren<SphereCollider>();
        SetMinMax();
    }

    //Sets the minimum and maximum values to use as a range dependant on its start location; saves editing inspector values each time.
    private void SetMinMax()
    {
        Collider collider = GetComponent<Collider>();
        yMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f);
        yMax = transform.localPosition.y;
    }

    //Check if the hand value is in the correct place and then check if it has been pressed.
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(hoverInteractor)
        {
            float newHandHeight = GetLocalYPosition(hoverInteractor.transform.position);
            float handDifference = previousHandHeight - newHandHeight;
            previousHandHeight = newHandHeight;

            float newPosition = transform.localPosition.y - handDifference;
            SetYPosition(newPosition);

            CheckPress();
        }
    }

    private float GetLocalYPosition(Vector3 position)
    {
        //Get the broad position of what we are adding in and converting it to the localSpace like the button.
        Vector3 localPosition = transform.root.InverseTransformPoint(position);
        return localPosition.y;
    }

    private void SetYPosition(float position)
    {
        Vector3 newPosition = transform.localPosition;

        //Make sure to clamp.
        newPosition.y = Mathf.Clamp(position, yMin, yMax);
        transform.localPosition = newPosition;
    }

    private void CheckPress()
    {
        bool inPosition = InPosition();

        if(inPosition && inPosition != previousPress) //If we are in position...
        {
            OnPress.Invoke();

            previousPress = inPosition;
        }

        else
        {
            OnDepress.Invoke();
        }
    }

    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMin + 0.01f);

        //If value is within clamped range (very close to fully up or completely up), the button is in position.
        return transform.localPosition.y == inRange;
    }

    public void EnableCollision()
    {
        sphere.enabled = true;
    }

    public void DisableCollision()
    {
        sphere.enabled = false;
    }
}