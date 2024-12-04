using UnityEngine;

public class VehicleMotionCompensation : MonoBehaviour
{
    public Transform carTransform; // IMU �������� ����(�ڵ��� Transform)
    public Transform xrOriginTransform; // XR Origin Transform (VR�� ����)
    public Transform headTransform; // Oculus�� ī�޶� (�Ӹ� ��ġ)

    private Quaternion initialCarRotation; // �ʱ� �ڵ����� ȸ��
    private Quaternion initialVROriginRotation; // �ʱ� XR Origin�� ȸ��

    void Start()
    {
        // �ʱ� �ڵ����� VR Origin�� ȸ�� ����
        initialCarRotation = carTransform.rotation;
        initialVROriginRotation = xrOriginTransform.rotation;
    }

    void Update()
    {
        // �ڵ��� ȸ���� �����Ͽ� XR Origin�� �ݿ�
        Quaternion carRotationDelta = Quaternion.Inverse(initialCarRotation) * carTransform.rotation;

        // XR Origin�� �ڵ��� ȸ�� ���� �ݿ�
        xrOriginTransform.rotation = initialVROriginRotation * Quaternion.Inverse(carRotationDelta);

        // ����� �Ӹ� ȸ��(Oculus)�� ��������� �ݿ�
        Vector3 headLocalRotation = headTransform.localEulerAngles;
        headTransform.localEulerAngles = new Vector3(headLocalRotation.x, headLocalRotation.y, headLocalRotation.z);
    }
}
