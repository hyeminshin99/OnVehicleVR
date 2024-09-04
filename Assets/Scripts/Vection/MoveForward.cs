using UnityEngine;
using KUsystem.Sensor;

public class MoveForward : MonoBehaviour
{
    private enum ETypeOfInput
    {
        OBD2 = 0,
        Manual = 1,
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

        transform.Translate(new Vector3(mSpeed * Time.deltaTime, 0.0f, 0.0f));
    }
};
