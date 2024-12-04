using UnityEngine;

public class BodyDirectionLock : MonoBehaviour
{
    public Transform xrOriginTransform; // XR Origin Transform (사용자 기준)
    public Transform carTransform; // 자동차 Transform (IMU 기준)
    public Transform screenTransform; // 가상 환경의 스크린 Transform

    private Quaternion initialBodyRotation; // 초기 몸 방향 저장
    private Vector3 initialBodyPosition; // 초기 몸 위치 저장

    void Start()
    {
        // Play 시작 시 초기 몸 방향 저장
        initialBodyRotation = Quaternion.Inverse(carTransform.rotation) * xrOriginTransform.rotation;
        initialBodyPosition = xrOriginTransform.position;

        // 스크린 방향에 XR Origin이 바라보도록 설정
        Vector3 targetDirection = screenTransform.position - xrOriginTransform.position;
        targetDirection.y = 0; // 수평 방향만 고려
        xrOriginTransform.rotation = Quaternion.LookRotation(targetDirection);
    }

    void Update()
    {
        // 차량의 Yaw 회전만 반영하여 XR Origin의 회전 고정
        float carYawRotation = carTransform.eulerAngles.y;

        // 스크린을 바라보는 방향 고정
        Vector3 screenDirection = screenTransform.position - xrOriginTransform.position;
        screenDirection.y = 0; // 수평 방향만 고려
        Quaternion screenRotation = Quaternion.LookRotation(screenDirection);

        // 차량 회전 값에 맞게 몸 방향 보정 (Yaw만 반영)
        xrOriginTransform.rotation = screenRotation * Quaternion.Euler(0, carYawRotation, 0);

        // 몸 위치는 초기화된 위치로 고정
        xrOriginTransform.position = initialBodyPosition;

        // 차량의 위아래 흔들림을 그대로 반영하도록 설정
        Vector3 headLocalRotation = xrOriginTransform.localEulerAngles;
        headLocalRotation.x = 0; // Pitch (위아래 회전) 그대로 유지
        headLocalRotation.z = 0; // Roll (좌우 기울기) 그대로 유지
        xrOriginTransform.localEulerAngles = headLocalRotation;
    }
}
