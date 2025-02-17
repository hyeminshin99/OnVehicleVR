using UnityEngine;

public class VehicleMotionCompensation : MonoBehaviour
{
    public Transform carTransform; // IMU 데이터의 기준(자동차 Transform)
    public Transform xrOriginTransform; // XR Origin Transform (VR의 기준)
    public Transform headTransform; // Oculus의 카메라 (머리 위치)

    private Quaternion initialCarRotation; // 초기 자동차의 회전
    private Quaternion initialVROriginRotation; // 초기 XR Origin의 회전
    private bool shouldApplyHeadRotation = false; // 머리 회전 적용 여부 플래그

    void Start()
    {
        // 초기 자동차와 XR Origin의 회전 저장
        initialCarRotation = carTransform.rotation;
        initialVROriginRotation = xrOriginTransform.rotation;
    }

    void _FixedUpdate()
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

        // ResetInitialRotation 호출 시 머리 회전 한 번 반영
        if (shouldApplyHeadRotation)
        {
            Vector3 newHeadLocalRotation = headTransform.localEulerAngles;
            headTransform.localEulerAngles = new Vector3(newHeadLocalRotation.x, newHeadLocalRotation.y, newHeadLocalRotation.z);

            shouldApplyHeadRotation = false; // 플래그 초기화
        }
    }

    public void ResetInitialRotation()
    {
        // 현재 자동차와 XR Origin의 회전을 초기값으로 재설정
        initialCarRotation = carTransform.rotation;

        // 현재 사용자의 머리 방향 기준으로 XR Origin 보정
        Quaternion currentHeadRotation = Quaternion.Euler(0, headTransform.eulerAngles.y, 0); // 머리의 Yaw만 반영
        initialVROriginRotation = xrOriginTransform.rotation * Quaternion.Inverse(currentHeadRotation);

        // 오른쪽으로 90도 보정 문제 해결
        initialVROriginRotation *= Quaternion.Euler(0, -90, 0); // 시선 조정을 위해 90도 보정 추가
        
        // 머리 회전 반영 플래그 활성화
        shouldApplyHeadRotation = true;

        Debug.Log("Environment orientation reset!");
    }
}
