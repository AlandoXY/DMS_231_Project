using Sirenix.OdinInspector;
using UnityEngine;

namespace Alando.Features.MyFPController
{
    public class CameraController : MonoBehaviour
    {
        [Title("General")]
        [SerializeField] private bool lockCursor = true;
        [SerializeField] private Transform playerBody;

        [Title("Camera")]
        [SerializeField] private Camera camera;
        [SerializeField] private float mouseSensitivity = 3.5f;
        [SerializeField] private Vector2 cameraLimit = new Vector2(-90f, 90f);
        [SerializeField, Range(0.0f, 0.1f)] private float mouseSmoothTime = 0.03f;



        private float cameraPitch;
        private Vector2 curMouseDelta = Vector2.zero;
        private Vector2 curMouseDeltaVelocity = Vector2.zero;
        private Vector3 originPos;

        private void Awake()
        {

        }

        private void Start()
        {
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = true;
            }

            originPos = transform.localPosition;
        }

        private void Update()
        {
            UpdateMouseLook();
        }

        private void UpdateMouseLook()
        {
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            curMouseDelta = Vector2.SmoothDamp(curMouseDelta, targetMouseDelta, ref curMouseDeltaVelocity, mouseSmoothTime);

            cameraPitch -= curMouseDelta.y * mouseSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, cameraLimit.x, cameraLimit.y);

            transform.localEulerAngles = Vector3.right * cameraPitch;
            playerBody.Rotate(Vector3.up * curMouseDelta.x * mouseSensitivity);

        }

        public void SetCameraFOV(float num)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, num, 4f * Time.deltaTime);
        }


        public void HeadBob(float _z, float _x_intensity, float y_intensity, float speed)
        {
            Vector3 targetPos = originPos + new Vector3(Mathf.Cos(_z) * _x_intensity, Mathf.Sin(_z * 2) * y_intensity, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * speed);
        }




        /// <summary>
        /// Camera 增加后坐力偏移
        /// </summary>
        /// <param name="recoil">x为水平后座，y为垂直后座</param>
        public void AddWeaponRecoil(Vector2 recoil)
        {
            //camX += recoil.x;
            //camY += recoil.y;
        }


    }
}