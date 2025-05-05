using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class BikeController : NetworkBehaviour
{
    [Header("Power/Braking")]
    [SerializeField] float motorForce = 1500f;
    [SerializeField] float brakeForce = 3000f;
    public Vector3 COG;

    [Header("Steering")]
    [SerializeField] float maxSteeringAngle = 35f;
    [Range(0f, 1f)] [SerializeField] float steerReductorAmount = 0.5f;
    [Range(0.001f, 1f)] [SerializeField] float turnSmoothing = 0.5f;

    [Header("Lean")]
    [SerializeField] float maxLeanAngle = 45f;
    [Range(0.001f, 1f)] [SerializeField] float leanSmoothing = 0.5f;

    [Header("References")]
    public Transform handle;
    [SerializeField] WheelCollider frontWheel;
    [SerializeField] WheelCollider backWheel;
    [SerializeField] Transform frontWheelTransform;
    [SerializeField] Transform backWheelTransform;
    [SerializeField] TrailRenderer frontTrail;
    [SerializeField] TrailRenderer rearTrail;
    [SerializeField] CameraController cameraController;

    private ContactProvider frontContact;
    private ContactProvider rearContact;
    private Rigidbody rb;
    private InputAction playerControls;
    private Vector2 moveDirection;
    private bool braking;
    private bool boosted = false;
    private float currentSteeringAngle;
    private float currentMaxSteeringAngle;
    private float currentLeanAngle;
    private float targetLeanAngle;

    void OnEnable()
    {
        if (isOwned)
        {
            playerControls = new InputAction(type: InputActionType.Value, binding: "<Gamepad>/leftStick");
            playerControls.AddCompositeBinding("Dpad")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            playerControls.Enable();
        }
    }

    void OnDisable()
    {
        if (isOwned) playerControls.Disable();
    }

    void Start()
    {
        if (!isOwned) return;

        rb = GetComponent<Rigidbody>();
        frontContact = frontTrail.transform.GetChild(0).GetComponent<ContactProvider>();
        rearContact = rearTrail.transform.GetChild(0).GetComponent<ContactProvider>();

        frontWheel.ConfigureVehicleSubsteps(6, 15, 18);
        backWheel.ConfigureVehicleSubsteps(6, 15, 18);
    }

    void Update()
    {
        if (!isOwned) return;
        GetInput();
    }

    void FixedUpdate()
    {
        if (!isOwned) return;

        HandleEngine();
        HandleSteering();
        LeanOnTurn();
        UpdateHandles();
        UpdateWheels();
        EmitTrail();
    }

    void GetInput()
    {
        moveDirection = playerControls.ReadValue<Vector2>();
        braking = Keyboard.current.spaceKey.isPressed;
    }

    void HandleEngine()
    {
        backWheel.motorTorque = braking ? 0f : moveDirection.y * motorForce;
        ApplyBraking(braking ? brakeForce : 0f);
    }

    void ApplyBraking(float force)
    {
        frontWheel.brakeTorque = force;
        backWheel.brakeTorque = force;
    }

    void HandleSteering()
    {
        MaxSteeringReductor();
        currentSteeringAngle = Mathf.Lerp(currentSteeringAngle, currentMaxSteeringAngle * moveDirection.x, turnSmoothing * 0.1f);
        frontWheel.steerAngle = currentSteeringAngle;
        targetLeanAngle = maxLeanAngle * -moveDirection.x;
    }

    void MaxSteeringReductor()
    {
        float t = (rb.velocity.magnitude / 30f) * steerReductorAmount;
        t = Mathf.Clamp01(t);
        currentMaxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 5f, t);
    }

    void UpdateHandles()
    {
        handle.localEulerAngles = new Vector3(handle.localEulerAngles.x, currentSteeringAngle, handle.localEulerAngles.z);
    }

    void LeanOnTurn()
    {
        Vector3 rot = transform.rotation.eulerAngles;

        if (rb.velocity.magnitude < 1f)
        {
            currentLeanAngle = Mathf.LerpAngle(currentLeanAngle, 0f, 0.1f);
        }
        else if (Mathf.Abs(currentSteeringAngle) < 0.5f)
        {
            currentLeanAngle = Mathf.LerpAngle(currentLeanAngle, 0f, leanSmoothing * 0.1f);
        }
        else
        {
            currentLeanAngle = Mathf.LerpAngle(currentLeanAngle, targetLeanAngle, leanSmoothing * 0.1f);
            rb.centerOfMass = new Vector3(rb.centerOfMass.x, COG.y, rb.centerOfMass.z);
        }

        transform.rotation = Quaternion.Euler(rot.x, rot.y, currentLeanAngle);
    }

    void UpdateWheels()
    {
        UpdateSingleWheel(frontWheel, frontWheelTransform);
        UpdateSingleWheel(backWheel, backWheelTransform);
    }

    void UpdateSingleWheel(WheelCollider wc, Transform wt)
    {
        wc.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wt.position = pos;
        wt.rotation = rot;
    }

    void EmitTrail()
    {
        if (braking)
        {
            frontTrail.emitting = frontContact.GetCOntact();
            rearTrail.emitting = rearContact.GetCOntact();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!isOwned) return;

        if (col.CompareTag("Boost Pad") && !boosted)
        {
            rb.AddForce(transform.forward * 350f, ForceMode.Impulse);
            boosted = true;
        }
        else if (col.CompareTag("Jump Pad") && !boosted)
        {
            rb.AddForce(transform.up * 250f, ForceMode.Impulse);
            boosted = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (!isOwned) return;
        if (col.CompareTag("Boost Pad")) boosted = false;
    }
}
