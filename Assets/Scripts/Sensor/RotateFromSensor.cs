using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFromSensor : MonoBehaviour
{
    public enum Condition
    {
        Condition1, // Globe 3D Model을 비활성화
        Condition2, // Globe 3D Model을 활성화하고 상대적인 각도로 회전
        Condition3  // Globe 3D Model을 활성화하고 상대적인 각도의 반대 방향으로 회전
    }

    public Condition condition;
    public Demo demoScript;

    private Transform globeTransform;
    private Vector3 initialAngles;
    private Vector3 mCalibrateValue;
    private bool initSetting = false;
    private bool mIsCalibrate = false;

    // Timer variables
    private float mCalibrateTimer = 0.0f;
    private const float CALIBRATE_TIME = 1.0f;  // @초 동안 보정

    void Start()
    {
        globeTransform = this.transform;
    }

    void Update()
    {
        if (demoScript != null)
        {
            string deviceId = demoScript.GetLatestDeviceId();

            if (string.IsNullOrEmpty(deviceId))
            {
                Debug.LogWarning("!!!Can not Find Device!!!!!");
                return;
            }

            // Device의 각도를 실시간으로 가져옵니다.
            Vector3 currentAngles = demoScript.GetDeviceAngles(deviceId);
            currentAngles = new Vector3(currentAngles.x, -currentAngles.z, currentAngles.y);

            // 아직 보정되지 않았다면 보정 진행
            if (!mIsCalibrate)
            {
                if (mCalibrateTimer > CALIBRATE_TIME)
                {
                    // 보정 값 저장 및 보정 완료
                    mCalibrateValue = currentAngles;
                    mIsCalibrate = true;
                    Debug.Log("Calibrated at: " + mCalibrateValue);
                }
                else
                {
                    // 보정 시간 타이머 증가
                    mCalibrateTimer += Time.deltaTime;
                }

                // 보정이 완료될 때까지 return
                return;
            }

            // 상대적인 회전값을 계산합니다 (보정된 값을 기준으로)
            Vector3 relativeAngles = currentAngles - mCalibrateValue;

            // 조건에 따라 동작을 다르게 수행
            switch (condition)
            {
                case Condition.Condition1:
                    globeTransform.gameObject.SetActive(false);
                    break;

                case Condition.Condition2:
                    globeTransform.gameObject.SetActive(true);
                    globeTransform.rotation = Quaternion.Euler(relativeAngles);
                    break;

                case Condition.Condition3:
                    globeTransform.gameObject.SetActive(true);
                    globeTransform.rotation = Quaternion.Euler(-relativeAngles);
                    break;
            }
        }
    }

    // 외부에서 호출할 수 있는 초기화 함수
    public void ResetCalibration()
    {
        mCalibrateTimer = 0.0f;
        mIsCalibrate = false;
        Debug.Log("Calibration reset. Recalibrating...");
    }
}
