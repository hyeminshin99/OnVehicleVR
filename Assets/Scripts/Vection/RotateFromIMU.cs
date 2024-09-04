using SeungHoon.Sensor;
using UnityEngine;

namespace SeungHoon.Vection
{
    [RequireComponent(typeof(IMU))]
    public class RotateFromIMU : MonoBehaviour
    {
        private IMU mIMU = null;
        private Vector3 mCalibrateValue = Vector3.zero;

        private bool mIsCalibrate = false;

        private float mCalibrateTimer = 0.0f;
        private const float CALIBRATE_TIME = 2.0f;

        private void Start()
        {
            mIMU = GetComponent<IMU>();
        }

        private void Update()
        {
            if (mIsCalibrate == false)
            {
                if(mCalibrateTimer > CALIBRATE_TIME)
                {
                    mCalibrateValue = mIMU.EulerRawData;
                    mIsCalibrate = true;
                    Debug.Log("Calibrated");
                }
                mCalibrateTimer += Time.deltaTime;
                return;
            }

            transform.rotation = Quaternion.Euler(
                mIMU.EulerRawData - mCalibrateValue);
        }
    }

}
