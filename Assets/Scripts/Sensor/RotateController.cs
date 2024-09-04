using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
    public enum Condition
    {
        Condition1, // Globe 3D Model을 비활성화
        Condition2, // Globe 3D Model을 활성화하고 받아온 각도로 회전
        Condition3  // Globe 3D Model을 활성화하고 받아온 각도의 반대 방향으로 회전
    }

    public Condition condition;
    public Demo demoScript; // Demo 스크립트를 참조할 수 있는 변수

    private Transform globeTransform;

    private Vector3 initialAngles;     // 처음에 받은 각도
    private bool initSetting = false; // 첫 각도를 받았는지 여부

    private Vector3 smoothedAngles;
    public float smoothingFactor = 0.1f;

    public GameObject AObject;

    void Start()
    {
        // Globe3DModel의 Transform을 가져옵니다. (이 스크립트가 Globe3DModel에 붙어있다면 바로 transform 사용 가능)
        globeTransform = AObject.transform;
    }

    void Update()
    {
        if (demoScript != null)
        {
            // Demo 스크립트에서 가장 최근에 발견된 디바이스의 ID를 가져옵니다.
            string deviceId = demoScript.GetLatestDeviceId();

            if (string.IsNullOrEmpty(deviceId))
            {
                Debug.Log("!!!Can not Find Device!!!!!");
                return;
            }

            Vector3 angles = demoScript.GetDeviceAngles(deviceId);
            // 센서의 Y축과 Z축을 Unity의 Z축과 Y축으로 교환
            angles = new Vector3(angles.x, -angles.z, angles.y);

            if (!initSetting)
            {
                InitAngles(angles);
            }
            smoothedAngles = Vector3.Lerp(smoothedAngles, angles, smoothingFactor);
            Vector3 relativeAngles = smoothedAngles - initialAngles;

            switch (condition)
            {
                case Condition.Condition1:
                    // Globe 3D Model을 비활성화
                    globeTransform.gameObject.SetActive(false);
                    break;

                case Condition.Condition2:
                    // Globe 3D Model을 활성화하고, 받아온 각도로 회전
                    globeTransform.gameObject.SetActive(true);
                    globeTransform.rotation = Quaternion.Euler(relativeAngles);
                    break;

                case Condition.Condition3:
                    // Globe 3D Model을 활성화하고, 받아온 각도의 반대 방향으로 회전
                    globeTransform.gameObject.SetActive(true);
                    globeTransform.rotation = Quaternion.Euler(-relativeAngles);
                    break;
            }
        }
    }

    void InitAngles(Vector3 currentAngles)
    {
        initialAngles = currentAngles;  // 초기 각도 저장
        smoothedAngles = currentAngles;
        initSetting = true;  // 초기 설정 완료
        Debug.Log("Initial Angles Set: " + initialAngles);
    }

    //Re-init UI Buttion
    public void ResetAngles()
    {
        if (demoScript != null)
        {
            // Demo 스크립트에서 가장 최근에 발견된 디바이스의 ID를 가져옵니다.
            string deviceId = demoScript.GetLatestDeviceId();

            // 만약 deviceId가 비어있다면 반환
            if (string.IsNullOrEmpty(deviceId))
            {
                Debug.LogWarning("!!!Can not Find Device!!!!!");
                return;
            }

            // Device의 각도를 실시간으로 가져옵니다.
            Vector3 currentAngles = demoScript.GetDeviceAngles(deviceId);
            currentAngles = new Vector3(currentAngles.x, -currentAngles.z, currentAngles.y);

            // 현재 각도를 새로운 초기 각도로 설정
            InitAngles(currentAngles);
        }
    }
}
