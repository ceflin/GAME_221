using UnityEngine;
using System.Collections;

public class CharacterGravity : MonoBehaviour {

    private Transform characterBase;

    void Awake()
    {
        characterBase = transform.Find("Base");
    }

	// Use this for initialization
	void Start () {
	
	}
	
    void Update()
    {

    }

	// Update is called once per frame
	void LateUpdate () {
	
        if (!IsGrounded)
        {
            
        }

	}

    bool IsGrounded
    {
        get
        {
            return Physics.Raycast(characterBase.position, Vector3.down, 0.05f, LayerMask.NameToLayer("Surface"));
        }
    }
}
