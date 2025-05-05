using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target; // Assign your player or bike here
    public Vector3 offset = new Vector3(0, 2, -5);
    public float rotationSpeed = 1.96f;
    public float pitchMin = -20f;
    public float pitchMax = 60f;

    private float yaw = 0f;
    private float pitch = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Get mouse or right stick input
        float horizontal = Input.GetAxis("Mouse X") + Input.GetAxis("RightStickHorizontal");
        float vertical = -Input.GetAxis("Mouse Y") + -Input.GetAxis("RightStickVertical");

        // Apply rotation
        yaw += horizontal * rotationSpeed;
        pitch += vertical * rotationSpeed;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Rotate and position camera
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target);
    }
}
