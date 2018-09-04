using UnityEngine;
using System.Collections;

public class SteeringAlign : MonoBehaviour {

    public Transform target;

    public float targetRange = 2;
    public float outerRange = 30;

    private CharacterSteeringMotor steeringMotor;

    // Use this for initialization
    void Start()
    {
        steeringMotor = GetComponent<CharacterSteeringMotor>();
    }

	// Update is called once per frame
	void Update () {

        if (target != null)
        {
            DelegatedSteer(steeringMotor, target.forward, targetRange, outerRange);
        }
	}

    public static void DelegatedSteer(ISteerable steerable, Vector3 targetDirection)
    {
        DelegatedSteer(steerable, targetDirection, steerable.AlignmentInnerRadius, steerable.AlignmentOuterRadius);
    }

    public static void DelegatedSteer(ISteerable steerable, Vector3 targetDirection, float targetRange, float outerRange)
    {

        float angle = Vector3.Angle(targetDirection, steerable.Forward);
        Vector3 cross = Vector3.Cross(targetDirection, steerable.Forward);
        angle *= angle * cross.y < 0 ? 1 : -1;

        // If we're within targetRange, we don't need to adjust.
        if (Mathf.Abs(angle) < targetRange) return;

        // If we're outside the slowdown range, we want maximum turning.
        // Assume we are.
        float targetRotation = steerable.MaxRotation;
        if (Mathf.Abs(angle) < outerRange)
        {
            targetRotation *= Mathf.Abs(angle) / outerRange;
        }
        targetRotation *= Mathf.Abs(angle) / angle;

        float adjustment = targetRotation - steerable.Rotation;

        steerable.Steer(adjustment);

    }
}
