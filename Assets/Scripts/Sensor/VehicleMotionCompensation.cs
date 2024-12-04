using UnityEngine;

public class VehicleMotionCompensation : MonoBehaviour
{
    public Transform carTransform; // IMU 데이터의 기준(자동차 Transform)
    public Transform xrOriginTransform; // XR Origin Transform (VR의 기준)
    public Transform headTransform; // Oculus의 카메라 (머리 위치)

    private Quaternion initialCarRotation; // 초기 자동차의 회전
    private Quaternion initialVROriginRotation; // 초기 XR Origin의 회전

    void Start()
    {
        // 초기 자동차와 XR Origin의 회전 저장
        initialCarRotation = carTransform.rotation;
        initialVROriginRotation = xrOriginTransform.rotation;
    }

    void Update()
    {
        // 자동차 회전 델타 계산
        Quaternion carRotationDelta = Quaternion.Inverse(initialCarRotation) * carTransform.rotation;

        // 자동차 회전 델타에서 Yaw(좌우 회전)만 추출
        Vector3 carEulerAngles = carRotationDelta.eulerAngles;
        float yaw = carEulerAngles.y; // Yaw(좌우 회전)만 추출

        // XR Origin의 회전 보정 (Yaw만 적용)
        Quaternion yawRotation = Quaternion.Euler(0, yaw, 0);
        xrOriginTransform.rotation = initialVROriginRotation * Quaternion.Inverse(yawRotation);

        // 사용자 머리 회전(Oculus)은 그대로 유지
        Vector3 headLocalRotation = headTransform.localEulerAngles;
        headTransform.localEulerAngles = new Vector3(headLocalRotation.x, headLocalRotation.y, headLocalRotation.z);
    }
}
