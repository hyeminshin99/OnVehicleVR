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
        // 초기 자동차와 VR Origin의 회전 저장
        initialCarRotation = carTransform.rotation;
        initialVROriginRotation = xrOriginTransform.rotation;
    }

    void Update()
    {
        // 자동차 회전을 보정하여 XR Origin에 반영
        Quaternion carRotationDelta = Quaternion.Inverse(initialCarRotation) * carTransform.rotation;

        // XR Origin에 자동차 회전 보정 반영
        xrOriginTransform.rotation = initialVROriginRotation * Quaternion.Inverse(carRotationDelta);

        // 사용자 머리 회전(Oculus)은 상대적으로 반영
        Vector3 headLocalRotation = headTransform.localEulerAngles;
        headTransform.localEulerAngles = new Vector3(headLocalRotation.x, headLocalRotation.y, headLocalRotation.z);
    }
}
