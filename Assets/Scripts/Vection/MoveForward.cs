using UnityEngine;
using KUsystem.Sensor;

public class MoveForward : MonoBehaviour
{
    private enum ETypeOfInput
    {
        OBD2 = 0,
        Manual = 1,
        KKL = 2
    }

    [SerializeField]
    private ETypeOfInput mInputType;

    [SerializeField]
    private float mDesiredSpeed;

    private IMU mIMU;

    private float mSpeed;
    public float Speed
    {
        set
        {
            mSpeed = value;
        }
    }

    private void Start()
    {
        if(mInputType == ETypeOfInput.OBD2)
        {
            mIMU = FindObjectOfType<IMU>();
        }
        else if (mInputType == ETypeOfInput.Manual)
        {
            // �������� Speed�� ����
            mSpeed = mDesiredSpeed;
        }
        else
        {
            Debug.LogError("Input type is not defined.");
        }
    }

    void Update()
    {
        if(mInputType == ETypeOfInput.OBD2)
        {
            mSpeed = mIMU.VehicleSpeed;
        }
        else if (mInputType == ETypeOfInput.KKL && SpeedManager.Instance != null)
        {
            // SpeedManager로부터 KKL 센서 속도 데이터를 받아옵니다.
            mSpeed = SpeedManager.Instance.Speed; //Input type is not defined. 라는 에러 뜸.
        }


        transform.Translate(new Vector3(mSpeed * Time.deltaTime, 0.0f, 0.0f));
    }
};
