using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Alando.Features.MyFPController
{
    public enum MovementState
    {
        Idle, Walk, Run, Crouch
    }
    public class PlayerMovementController : MonoBehaviour
    {
        [Title("Camera")]
        [SerializeField] private CameraController cameraController;

        [Title("General")]
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float crouchSpeed = 1f;

        [Title("Slope")]
        [SerializeField] private float slopeForce;
        [SerializeField] private float slopeForceRayLength;

        [Title("Walk")]
        [SerializeField, Range(0.0f, 0.5f)] private float moveSmoothTime = 0.3f;

        [Title("Run")]
        [SerializeField] private float runBuildUpSpeed;

        [Title("Crouch")]
        [SerializeField] private float crouchPercent;

        [Title("Jump")]
        [SerializeField] private AnimationCurve jumpFallOff;
        [SerializeField] private float jumpMultiplier;

        [Title("Ground")]
        [SerializeField] private float gravity = -13.0f;


        [Title("DEBUG")]
        [SerializeField, ReadOnly] private float debugMovementSpeed;
        [SerializeField, ReadOnly] private float debugVelocity;
        [SerializeField, ReadOnly] private string debugMovementState;


        private float velocityY;
        private CharacterController controller;

        private Vector2 curDir = Vector2.zero;
        private Vector2 curDirVelocity = Vector2.zero;

        private bool isJumping;
        private bool isCrouching;

        private float movementSpeed;

        private float movementCount;
        private float idleCount;
        private MovementState movementState;

        private Vector3 originPoint;


        private void Start()
        {
            controller = GetComponent<CharacterController>();

        }



        private void Update()
        {
            UpdateMovement();
            UpdateDebugInfo();
        }

        #region Custom Method

        private void UpdateMovement()
        {

            Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            targetDir.Normalize();

            curDir = Vector2.SmoothDamp(curDir, targetDir, ref curDirVelocity, moveSmoothTime);

            if (controller.isGrounded)
            {
                velocityY = 0.0f;
            }

            velocityY += gravity * Time.deltaTime;


            Vector3 velocity = (transform.forward * curDir.y + transform.right * curDir.x) * movementSpeed + Vector3.up * velocityY;
            controller.Move(velocity * Time.deltaTime);


            if ((targetDir.x != 0 || targetDir.y != 0) && OnSlope())
            {
                controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
            }


            SetMovementSpeed();
            SetJump();
            if (targetDir.magnitude == 0)
            {
                movementState = MovementState.Idle;
            }

            if (!isJumping)
            {
                SetHeadBob();
            }

            debugVelocity = targetDir.magnitude;
        }


        private void SetJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                isJumping = true;
                StartCoroutine(JumpEvent());
            }

        }

        private IEnumerator JumpEvent()
        {
            controller.slopeLimit = 90f;
            float timeInAir = 0.0f;

            do
            {
                float jumpForce = jumpFallOff.Evaluate(timeInAir);
                controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
                timeInAir += Time.deltaTime;
                yield return null;
            } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

            controller.slopeLimit = 50f;
            movementState = MovementState.Idle;
            isJumping = false;
        }

        private bool OnSlope()
        {
            if (isJumping) return false;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeForceRayLength))
            {
                if (hit.normal != Vector3.up)
                    return true;
            }

            return false;
        }

        private void SetMovementSpeed()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
                cameraController.SetCameraFOV(70f);
                movementState = MovementState.Run;
            }
            else
            {
                movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
                cameraController.SetCameraFOV(60f);
                movementState = MovementState.Walk;
            }
        }


        private void SetCrouch()
        {
            if (Input.GetKey(KeyCode.LeftControl) && !isCrouching)
            {
                StartCoroutine(CrouchEvent());
            }
        }

        private IEnumerator CrouchEvent()
        {

            yield return null;
        }




        private void UpdateDebugInfo()
        {
            debugMovementSpeed = movementSpeed;
            debugMovementState = movementState.ToString();
        }

        private void SetHeadBob()
        {
            switch (movementState)
            {
                case MovementState.Idle:
                    cameraController.HeadBob(idleCount, 0.025f, 0.025f, 2f);
                    idleCount += Time.deltaTime;
                    break;
                case MovementState.Walk:
                    cameraController.HeadBob(movementCount, 0.1f, 0.1f, 8f);
                    movementCount += Time.deltaTime * 6f;
                    break;
                case MovementState.Run:
                    cameraController.HeadBob(movementCount, 0.15f, 0.08f, 10f);
                    movementCount += Time.deltaTime * 8f;
                    break;
                case MovementState.Crouch:
                    cameraController.HeadBob(movementCount, 0.015f, 0.015f, 2f);
                    movementCount += Time.deltaTime;
                    break;
            }
        }

        #endregion

        /*
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            originPoint = transform.position + controller.center;
            Vector3 pos = originPoint + Vector3.down * (controller.center.y - 0.5f);
            Gizmos.DrawSphere(pos, 0.5f);
        }
        */


    }
}
