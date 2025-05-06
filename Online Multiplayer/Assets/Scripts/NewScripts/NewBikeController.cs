using UnityEngine;

public class NewBikeController : MonoBehaviour
{
    float MoveInput, steerInput;
    public float MaxSpeed, acceleration, steerStrength, gravity;
    public Rigidbody SphereRB, BikeBody;
    [Range(1, 10)]
    public float brakingFactor;

    public float RayLength;
    public LayerMask DrivableSurface;
    RaycastHit hit;

    public float zTiltAngle = 75f, currentVelocityOffset;
    public Vector3 velocity;
    public GameObject bikeModel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RayLength = SphereRB.GetComponent<SphereCollider>().radius + 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
        transform.position = SphereRB.transform.position;
        velocity = BikeBody.transform.InverseTransformDirection(BikeBody.linearVelocity);
        currentVelocityOffset = velocity.z / MaxSpeed;
    }
    
    private void FixedUpdate()
    {
        Movement();
    }
    
    void Movement()
    {
        if (Grounded())
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                Accelaration();
                Rotation();
            }
            Brake();
        }
        else
        {
            Gravity();
        }
        BikeTilt();
    }

    void Accelaration()
    {
        SphereRB.linearVelocity = Vector3.Lerp(SphereRB.linearVelocity, MaxSpeed * MoveInput * transform.forward, Time.fixedDeltaTime * acceleration);
    }

    void Rotation()
    {
        transform.Rotate(0, steerInput * MoveInput * steerStrength * Time.fixedDeltaTime, 0, Space.World);
    }

    void BikeTilt()
    {
        // Check if the bike is grounded
        if (Grounded())
        {
            float zRot = 0;

            // Calculate the z-axis (roll) tilt based on the steer input and some multiplier (zTiltAngle)
            zRot = -zTiltAngle * steerInput * currentVelocityOffset; // Adjust this calculation based on your needs

            // Calculate the tilt based on the surface normal
            Quaternion tiltRotation = Quaternion.FromToRotation(BikeBody.transform.up, hit.normal);

            // Add the z-rotation (roll) to the target rotation
            Quaternion targetRotation = tiltRotation * BikeBody.transform.rotation;

            // Apply the z-rotation by creating a new quaternion for roll
            targetRotation *= Quaternion.Euler(0, 0, zRot);  // This adds the roll to the tilt

            // Smoothly transition to the target rotation
            BikeBody.MoveRotation(Quaternion.Slerp(BikeBody.rotation, targetRotation, Time.fixedDeltaTime)); // Adjust the speed factor (5f)
        }
        else
        {
            // If not grounded, reset tilt to upright position
            Quaternion resetRotation = Quaternion.Euler(0, BikeBody.rotation.eulerAngles.y, 0); // Keep current yaw but reset pitch and roll
            BikeBody.MoveRotation(Quaternion.Slerp(BikeBody.rotation, resetRotation, Time.fixedDeltaTime * 10f)); // You can adjust the smoothing factor (10f) as needed
        }

        if (bikeModel != null)
        {
            float leanSpeedFactor = BikeBody.linearVelocity.magnitude / MaxSpeed;
            float visualLean = -zTiltAngle * steerInput * Mathf.Clamp01(leanSpeedFactor);

            // Create a smooth rotation around Z (roll)
            Quaternion targetVisualRotation = Quaternion.Euler(visualLean, bikeModel.transform.localEulerAngles.y, bikeModel.transform.localEulerAngles.z);

            // Apply with smoothing
            bikeModel.transform.localRotation = Quaternion.Slerp(bikeModel.transform.localRotation, targetVisualRotation, Time.fixedDeltaTime * 5f); // Adjust smoothing factor if needed
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SphereRB.linearVelocity = Vector3.Lerp(SphereRB.linearVelocity, Vector3.zero, brakingFactor * Time.fixedDeltaTime);
        }
    }

    bool Grounded()
    {
        if (Physics.Raycast(SphereRB.position, Vector3.down, out hit, RayLength, DrivableSurface))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Gravity()
    {
        SphereRB.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }
}
