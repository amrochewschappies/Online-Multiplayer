using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace rayzngames
{
    public class CameraController : MonoBehaviour
    {
        public InputAction PlayerControls;
        public Vector2 MoveDirection;

        public Transform target;
        [Space(10)]
        public float sensitivity;

        Transform camPos;
        Vector3 camOffset;
        Vector3 originalCamOffset;

        [SerializeField] float keepAtDist_Col = 0.75f;
        [SerializeField] LayerMask collidesWith;

        private bool isShaking = false;
        private float shakeDuration = 0f;
        private float shakeAmount = 0.1f;

        private void OnEnable()
        {
            PlayerControls.Enable();
        }

        private void OnDisable()
        {
            PlayerControls.Disable();
        }

        private void Start()
        {
            camPos = transform.GetChild(0);
            originalCamOffset = camPos.localPosition;
            camOffset = originalCamOffset;
            camPos.localPosition = camOffset;
        }

        void Update()
        {
            transform.position = target.position;
            ThirdPersonRotate();
            CheckCamCollisionOffset();
            ZoomCamera();

            // Handle camera shake (update the camera position)
            if (isShaking)
            {
                if (shakeDuration > 0)
                {
                    shakeDuration -= Time.deltaTime;
                    camPos.localPosition = originalCamOffset + Random.insideUnitSphere * shakeAmount;
                }
                else
                {
                    isShaking = false;
                    camPos.localPosition = originalCamOffset; // Reset to original position
                }
            }
        }

        void ZoomCamera()
        {
            camOffset.z += Input.mouseScrollDelta.y * 10 * Time.fixedDeltaTime;
        }

        private void ThirdPersonRotate()
        {
            MoveDirection = PlayerControls.ReadValue<Vector2>();
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Rotate the camera horizontally (Y rotation) based on Mouse X movement
            transform.Rotate(Vector3.up * MoveDirection.x * sensitivity);

            float currentXRotation = transform.eulerAngles.x;

            // Limit the vertical X rotation (MouseY) to avoid camera flipping
            if (currentXRotation > 180f)
            {
                currentXRotation -= 360f;
            }

            // Rotate the camera vertically based on mouse movement
            float clampedXRotation = Mathf.Clamp(currentXRotation - MoveDirection.y * sensitivity, -70f, 70f);

            // Apply the rotation
            transform.rotation = Quaternion.Euler(clampedXRotation, transform.eulerAngles.y, transform.rotation.z);
        }

        private void CheckCamCollisionOffset()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, camPos.TransformDirection(Vector3.back), out hit, Mathf.Abs(camOffset.z) + keepAtDist_Col, collidesWith))
            {
                Vector3 colCorrectedPos = hit.point;
                camPos.position = colCorrectedPos - camPos.TransformDirection(Vector3.back) * (keepAtDist_Col);
                Debug.DrawRay(transform.position, camPos.TransformDirection(Vector3.back) * hit.distance, Color.red);
            }
            else
            {
                camPos.localPosition = camOffset;
                Debug.DrawRay(transform.position, camPos.TransformDirection(Vector3.back) * 100, Color.blue);
            }
        }

        // Method to trigger camera shake
        public void TriggerShake(float amount, float duration)
        {
            shakeAmount = amount;
            shakeDuration = duration;
            isShaking = true;
        }
    }
}
