using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class MirrorThirdPersonController : NetworkBehaviour
{
    public Transform cam;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;
    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    private Rigidbody rb;
    private Vector3 inputDirection;

    public override void OnStartLocalPlayer()
    {
        // Set up camera follow for the local player only
        if (Camera.main != null)
        {
            Camera.main.GetComponentInParent<CameraFollow>().SetTarget(transform);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        inputDirection = new Vector3(h, 0, v).normalized;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            CmdJump();
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        if (inputDirection.magnitude >= 0.1f)
        {
            Vector3 moveDir = cam.forward * inputDirection.z + cam.right * inputDirection.x;
            moveDir.y = 0f;
            moveDir.Normalize();

            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    [Command]
    void CmdJump()
    {
        RpcJump();
    }

    [ClientRpc]
    void RpcJump()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}