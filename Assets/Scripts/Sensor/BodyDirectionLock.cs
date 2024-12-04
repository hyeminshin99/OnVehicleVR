using UnityEngine;

public class BodyDirectionLock : MonoBehaviour
{
    public Transform xrOriginTransform; // XR Origin Transform (����� ����)
    public Transform carTransform; // �ڵ��� Transform (IMU ����)
    public Transform screenTransform; // ���� ȯ���� ��ũ�� Transform

    private Quaternion initialBodyRotation; // �ʱ� �� ���� ����
    private Vector3 initialBodyPosition; // �ʱ� �� ��ġ ����

    void Start()
    {
        // Play ���� �� �ʱ� �� ���� ����
        initialBodyRotation = Quaternion.Inverse(carTransform.rotation) * xrOriginTransform.rotation;
        initialBodyPosition = xrOriginTransform.position;

        // ��ũ�� ���⿡ XR Origin�� �ٶ󺸵��� ����
        Vector3 targetDirection = screenTransform.position - xrOriginTransform.position;
        targetDirection.y = 0; // ���� ���⸸ ���
        xrOriginTransform.rotation = Quaternion.LookRotation(targetDirection);
    }

    void Update()
    {
        // ������ Yaw ȸ���� �ݿ��Ͽ� XR Origin�� ȸ�� ����
        float carYawRotation = carTransform.eulerAngles.y;

        // ��ũ���� �ٶ󺸴� ���� ����
        Vector3 screenDirection = screenTransform.position - xrOriginTransform.position;
        screenDirection.y = 0; // ���� ���⸸ ���
        Quaternion screenRotation = Quaternion.LookRotation(screenDirection);

        // ���� ȸ�� ���� �°� �� ���� ���� (Yaw�� �ݿ�)
        xrOriginTransform.rotation = screenRotation * Quaternion.Euler(0, carYawRotation, 0);

        // �� ��ġ�� �ʱ�ȭ�� ��ġ�� ����
        xrOriginTransform.position = initialBodyPosition;

        // ������ ���Ʒ� ��鸲�� �״�� �ݿ��ϵ��� ����
        Vector3 headLocalRotation = xrOriginTransform.localEulerAngles;
        headLocalRotation.x = 0; // Pitch (���Ʒ� ȸ��) �״�� ����
        headLocalRotation.z = 0; // Roll (�¿� ����) �״�� ����
        xrOriginTransform.localEulerAngles = headLocalRotation;
    }
}
