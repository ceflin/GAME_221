using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
    public enum RotationAxis { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxis axes = RotationAxis.MouseXAndY;

    public float sensitivityX = 15f;
    public float sensitivityY = 15f;

    public float minX = -360f;
    public float maxX = 360f;

    public float minY = -60f;
    public float maxY = 60f;

    float rotationX = 0f;
    float rotationY = 0f;

    private List<float> rotationArrayX = new List<float>();
    float rotationAverageX = 0f;

    private List<float> rotationArrayY = new List<float>();
    float rotationAverageY = 0f;

    public float frameCounter = 20f;

    Quaternion originalRotation;

    // Use this for initialization
    void Start()
    {
        //Rigidbody rb = GetComponent<Rigidbody>();
        //if (rb)
        //    rb.freezeRotation = true;

        CharacterController characterController = GetComponent<CharacterController>();
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxis.MouseXAndY)
        {
            rotationAverageX = 0f;
            rotationAverageY = 0f;

            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            rotationArrayX.Add(rotationX);
            rotationArrayY.Add(rotationY);

            if (rotationArrayX.Count >= frameCounter) rotationArrayX.RemoveAt(0);
            if (rotationArrayY.Count >= frameCounter) rotationArrayY.RemoveAt(0);

            for (int x = 0; x < rotationArrayX.Count; x++)
                rotationAverageX += rotationArrayX[x];
            for (int y = 0; y < rotationArrayY.Count; y++)
                rotationAverageY += rotationArrayY[y];

            rotationAverageX /= rotationArrayX.Count;
            rotationAverageY /= rotationArrayY.Count;

            rotationAverageX = ClampAngle(rotationAverageX, minX, maxX);
            rotationAverageY = ClampAngle(rotationAverageY, minY, maxY);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationAverageX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationAverageY, Vector3.left);

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxis.MouseX)
        {
            rotationAverageX = 0f;
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationArrayX.Add(rotationX);

            if (rotationArrayX.Count >= frameCounter)
                rotationArrayX.RemoveAt(0);

            for (int x = 0; x < rotationArrayX.Count; x++)
                rotationAverageX += rotationArrayX[x];

            rotationAverageX /= rotationArrayX.Count;
            rotationAverageX = ClampAngle(rotationAverageX, minX, maxX);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationAverageX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            rotationAverageY = 0f;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationArrayY.Add(rotationY);

            if (rotationArrayY.Count >= frameCounter)
                rotationArrayY.RemoveAt(0);

            for (int y = 0; y < rotationArrayY.Count; y++)
                rotationAverageY += rotationArrayY[y];

            rotationAverageY /= rotationArrayY.Count;
            rotationAverageY = ClampAngle(rotationAverageY, minY, maxY);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotationAverageY, Vector3.left);
            transform.localRotation = originalRotation * yQuaternion;
        }
    }

    public static float ClampAngle (float angle, float min, float max)
    {
        angle %= 360f;
        if (angle >= -360f && angle <= 360f)
        {
            if (angle < -360f)
                angle += 360f;
            if (angle > 360f)
                angle += -360f;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
