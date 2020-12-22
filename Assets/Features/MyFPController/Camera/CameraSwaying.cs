using System;
using UnityEngine;

namespace Alando.Features.MyFPController
{
    [Serializable]
    public class CameraSwaying
    {
        [Header("Sway Settings")]
        [SerializeField] private float swayAmount;

        [SerializeField] private float swaySpped;
        [SerializeField] private float returnSpeed;
        [SerializeField] private float ChangeDirectionMultiplier;
        [SerializeField] private AnimationCurve swayCurve = new AnimationCurve();

        #region Private Variables

        private Transform camTransform;
        private float scrollSpeed;

        private float curX; // x this frame
        private float preX; // x previous frame

        private bool diffDirection;

        #endregion

        #region Custom Methods

        public void Init(Transform cam)
        {
            camTransform = cam;
        }

        public void SwayPlayer(Vector3 inputVector, float rawXInput)
        {
            float xAmount = inputVector.x;
            curX = rawXInput;

            if (rawXInput != 0f)
            {
                if (curX != preX && preX != 0) { diffDirection = true; }
                float speedMultiplier = diffDirection ? ChangeDirectionMultiplier : 1f;
                scrollSpeed += (xAmount * swaySpped * Time.deltaTime * speedMultiplier);
            }
            else
            {
                if (curX == preX) { diffDirection = false; }

                scrollSpeed = Mathf.Lerp(scrollSpeed, 0f, Time.deltaTime * returnSpeed);
            }

            scrollSpeed = Mathf.Clamp(scrollSpeed, -1f, 1f);

            float swayFinalAmount;
            if (scrollSpeed < 0f)
            {
                swayFinalAmount = -swayCurve.Evaluate(scrollSpeed) * swayAmount;
            }
            else
            {
                swayFinalAmount = swayCurve.Evaluate(scrollSpeed) * swayAmount;
            }

            Vector3 swayVector;
            swayVector.z = swayFinalAmount;

            camTransform.localEulerAngles = new Vector3(camTransform.localEulerAngles.x, camTransform.localEulerAngles.y, swayVector.z);
            preX = curX;
        }

        #endregion
    }
}