using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public float acceleration = 20f;
    public float maxSpeed = 50f;
    public float turnSpeed = 100f;
    public float leanAngle = 15f;
    public float deceleration = 10f;

    private float currentSpeed = 0f;
    public GameObject MeshThing;

    void Update()
    {

        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Acceleration and braking
        if (moveInput > 0)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (moveInput < 0)
        {
            currentSpeed -= deceleration * Time.deltaTime;
        }
        else
        {
            // Gradual speed decay when no input
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Clamp speed
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed);

        // Move the motorcycle
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Steering
        if (currentSpeed != 0)
        {
            float turn = turnInput * turnSpeed * Time.deltaTime * Mathf.Sign(currentSpeed);
            transform.Rotate(Vector3.up * turn);

            // Lean effect (visual only)
            float targetLean = -turnInput * leanAngle;
            Vector3 localEuler = MeshThing.transform.localEulerAngles;
            localEuler.x = targetLean;
            MeshThing.transform.localEulerAngles = new Vector3(targetLean, MeshThing.transform.localEulerAngles.y, localEuler.z);

        }
    }
}
