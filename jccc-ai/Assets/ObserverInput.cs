using UnityEngine;
using System.Collections;

public class ObserverInput : MonoBehaviour {

    public LayerMask backgroundLayer;

    private enum ObserverInputState
    {
        Neutral,
        Panning,
        Rotating,
        Dollying
    }

    private ObserverInputState state = ObserverInputState.Neutral;
    private Vector3 lastMousePosition;

    // Rotating
    private Vector3 orbitCenter;
    public float rotationSpeed = 1.0f;

    // Panning
    public float panSpeed = 1.0f;

    // Dollying
    public float dollySpeed = 1.0f;
    public float minHeight = 10.0f;
    public float maxHeight = 20.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        switch (state)
        {
            case ObserverInputState.Neutral:
                if (Input.GetMouseButtonDown(0))
                {
                    // Enter rotating: Mark the spot we're looking at and our current mouse location and orbit
                    RaycastHit info;
                    if (Physics.Raycast(transform.position, transform.forward, out info, 1000, backgroundLayer))
                    {
                        orbitCenter = info.point;
                        state = ObserverInputState.Rotating;
                    }
                }
                else if (Input.GetMouseButtonDown(2))
                {
                    state = ObserverInputState.Panning;
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    state = ObserverInputState.Dollying;
                }
                else
                {
                    Dolly(Input.mouseScrollDelta.y * dollySpeed);
                }

                break;
            case ObserverInputState.Panning:
                if (Input.GetMouseButtonUp(2))
                {
                    // Back to neutral
                    state = ObserverInputState.Neutral;
                }
                else
                {
                    Vector3 proposedMotion = transform.right * -(Input.mousePosition.x - lastMousePosition.x);
                    proposedMotion += Vector3.Cross(transform.right, Vector3.up) * -(Input.mousePosition.y - lastMousePosition.y);
                    proposedMotion *= Time.deltaTime * transform.position.y / minHeight;

                    // Test for validity
                    RaycastHit info;
                    if (Physics.Raycast(transform.position + proposedMotion, transform.forward, out info, 1000, backgroundLayer))
                    {
                        transform.position += proposedMotion;
                    }

                    
                }
                break;
            case ObserverInputState.Rotating:
                if (Input.GetMouseButtonUp(0))
                {
                    state = ObserverInputState.Neutral;
                }
                else
                {
                    Vector3 current = transform.position - orbitCenter;
                    current = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime * (Input.mousePosition.x - lastMousePosition.x), Vector3.up) * current;
                    transform.position = orbitCenter + current;
                    transform.LookAt(orbitCenter, Vector3.up);
                }
                break;
            case ObserverInputState.Dollying:
                if (Input.GetMouseButtonUp(1))
                {
                    state = ObserverInputState.Neutral;
                }
                else
                {
                    float distance = Time.deltaTime * (Input.mousePosition.x - lastMousePosition.x);
                    Dolly(distance);
                }
                break;
        }

        lastMousePosition = Input.mousePosition;


    }

    private void Dolly(float distance)
    {
        Vector3 proposedDestination = transform.position + transform.forward * distance;
        if (proposedDestination.y < minHeight)
        {
            distance *= (transform.position.y - minHeight) / (transform.position.y - proposedDestination.y);
        }
        if (proposedDestination.y > maxHeight)
        {
            distance *= (maxHeight - transform.position.y) / (proposedDestination.y - transform.position.y);
        }
        transform.position += transform.forward * distance;
    }
}
