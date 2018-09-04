using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetectGround : MonoBehaviour {


    Dictionary<GameObject, bool> collisionContacts = new Dictionary<GameObject, bool>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        print("Ground detect trigger enter with " + other.gameObject.name);
        // We only collide with Surfaces
        if (!collisionContacts.ContainsKey(other.gameObject))
        {
            collisionContacts.Add(other.gameObject, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        print("Ground detect trigger exit with " + other.gameObject.name);
        if (collisionContacts.ContainsKey(other.gameObject))
        {
            collisionContacts.Remove(other.gameObject);
        }
    }

    public bool IsGrounded
    {
        get
        {
            print("Collision contacts " + collisionContacts.Count.ToString());
            return collisionContacts.Count > 0;
        }
    }

}
