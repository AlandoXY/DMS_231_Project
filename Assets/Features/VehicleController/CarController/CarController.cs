using UnityEngine;

namespace Assets.Features.VehicleController.CarController
{
    internal enum SpeedType
    {
        MPH, KPH
    }
    public class CarController : MonoBehaviour
    {
        private float aimHInput, aimVInput;
        private float curHInput, curVInput;



        [SerializeField] private float motorForce;
        [SerializeField] private float breakForce;
        [SerializeField] private Transform steeringWheelTransform;

        [Tooltip("FR,FL,RR,RL")]
        [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
        [Tooltip("FR,FL,RR,RL")]
        [SerializeField] private Transform[] wheelTransforms = new Transform[4];
        [SerializeField] private Vector3 centerOfMassOffset = Vector3.zero;
        [SerializeField] private float maxSteerAngle = 25f;
        [Range(0, 1)] [SerializeField] private float steerHelper; // 0 is raw physics , 1 the car will grip in the direction it is facing
        [Range(0, 1)] [SerializeField] private float tractionControl; // 0 is no traction control, 1 is full interference
        [SerializeField] private float fullTorqueOverAllWheels;
        [SerializeField] private float reverseTorque;
        [SerializeField] private float maxHandbrakeTorque;
        [SerializeField] private float downForce = 100f;
        [SerializeField] private SpeedType speedType;
        [SerializeField] private float topspeed = 200f;
        [SerializeField] private static int NoOfGears = 5;
        [SerializeField] private float revRangeBoundary = 1f;
        [SerializeField] private float slipLimit;
        [SerializeField] private float brakeTorque;

        private Quaternion[] wheelLocalRotations;
        private Vector3 prePos, curpos;
        private float curSteerAngle;
        private int gearNum;
        private float gearFactor;
        private float preRotation;
        private float curTorque;
        private Rigidbody carRigidbody;
        private const float reversingThreshold = 0.01f;

        public float CurrentSpeed { get { return carRigidbody.velocity.magnitude * 2.23693629f; } }


        private void Start()
        {
            wheelLocalRotations = new Quaternion[4];
            for (int i = 0; i < 4; i++)
            {
                wheelLocalRotations[i] = wheelTransforms[i].localRotation;
            }

            wheelColliders[0].attachedRigidbody.centerOfMass = centerOfMassOffset;

            maxHandbrakeTorque = float.MaxValue;
            carRigidbody = GetComponent<Rigidbody>();
            curTorque = fullTorqueOverAllWheels - (tractionControl * fullTorqueOverAllWheels);
        }


        private void FixedUpdate()
        {
            GetInput();
            Move(curHInput, curVInput, curVInput);
        }

        private static float ULerp(float from, float to, float value)
        {
            if (Mathf.Abs(to - from) < 0.0000001f)
            {
                return to;
            }
            else
            {
                return (1.0f - value) * from + value * to;
            }

        }


        private void GetInput()
        {
            aimHInput = Input.GetAxisRaw("Horizontal");
            aimVInput = Input.GetAxisRaw("Vertical");
            float t = 0.2f;
            curHInput = ULerp(curHInput, aimHInput, t);
            curVInput = ULerp(curVInput, aimVInput, t);
            //Debug.Log($"curInput: {curHInput} | {curVInput}");
            //Debug.Log($"aimInput: {aimHInput} | {aimVInput}");
        }

        private void ApplyDrive(float accel, float footbrake)
        {
            float thrustTorque;
            thrustTorque = accel * (curTorque / 4f);
            //Debug.Log($"thrustTorque:{thrustTorque}");
            //Debug.Log($"w:{wheelColliders[1].motorTorque}");
            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].motorTorque = thrustTorque;
            }

            for (int i = 0; i < 4; i++)
            {
                if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, carRigidbody.velocity) < 50f)
                {
                    wheelColliders[i].brakeTorque = brakeTorque * footbrake;
                }
                else if (footbrake > 0)
                {
                    wheelColliders[i].brakeTorque = 0f;
                    wheelColliders[i].motorTorque = -reverseTorque * footbrake;
                }
            }
        }

        private void SteerHelper()
        {
            for (int i = 0; i < 4; i++)
            {
                WheelHit wheelHit;
                wheelColliders[i].GetGroundHit(out wheelHit);
                if (wheelHit.normal == Vector3.zero)
                {
                    return; // 如果车轮不在地面上，则不要重新调整刚体速度；
                }
            }
            // 防止突然转向。万向轮问题
            if (Mathf.Abs(preRotation - transform.eulerAngles.y) < 10f)
            {
                var turnadjust = (transform.eulerAngles.y - preRotation) * steerHelper;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                carRigidbody.velocity = velRotation * carRigidbody.velocity;
            }

            preRotation = transform.eulerAngles.y;
        }

        // 增加抓地力
        private void AddDownForce()
        {
            wheelColliders[0].attachedRigidbody.AddForce(transform.up * (downForce * wheelColliders[0].attachedRigidbody.velocity.magnitude));
        }

        private void TractionControl()
        {
            WheelHit wheelHit;
            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
            }
        }

        private void AdjustTorque(float forwardSlip)
        {
            if (forwardSlip >= slipLimit && curTorque >= 0)
            {
                curTorque -= 10 * tractionControl;
            }
            else
            {
                curTorque += 10 * tractionControl;
                if (curTorque > fullTorqueOverAllWheels)
                {
                    curTorque = fullTorqueOverAllWheels;
                }
            }
        }

        private void CapSpeed()
        {
            float speed = carRigidbody.velocity.magnitude;
            switch (speedType)
            {
                case SpeedType.MPH:
                    speed *= 2.23693629f;
                    if (speed > topspeed)
                    {
                        carRigidbody.velocity = (topspeed / 2.23693629f) * carRigidbody.velocity.normalized;
                    }
                    break;
                case SpeedType.KPH:
                    speed *= 3.6f;
                    if (speed > topspeed)
                    {
                        carRigidbody.velocity = (topspeed / 3.6f) * carRigidbody.velocity.normalized;
                    }
                    break;
            }
        }

        public void Move(float steering, float accel, float footbrake)
        {
            for (int i = 0; i < 4; i++)
            {
                Quaternion quat;
                Vector3 pos;
                wheelColliders[i].GetWorldPose(out pos, out quat);
                wheelTransforms[i].position = pos;
                wheelTransforms[i].rotation = quat;
            }

            steering = Mathf.Clamp(steering, -1, 1);
            accel = Mathf.Clamp(accel, 0, 1);
            footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);

            curSteerAngle = steering * maxSteerAngle;
            wheelColliders[0].steerAngle = curSteerAngle;
            wheelColliders[1].steerAngle = curSteerAngle;
            steeringWheelTransform.localEulerAngles = Vector3.back * (curSteerAngle * 3f);

            SteerHelper();
            ApplyDrive(accel, footbrake);
            CapSpeed();

            //CalculateRevs();
            //GearChanging();
            AddDownForce();
            //CheckForWheelSpin();
            TractionControl();

        }





    }
}