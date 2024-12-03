using UnityEngine;

namespace KUsystem.Sensor
{
    public class IMU : MonoBehaviour
    {
        // �迡�� ������ �� => BNO080
        // ������ ������ �� => OBD2
        private enum ETypeOfIMU
        {
            OBD2 = 0,
            BNO080 = 1,
        }

        [SerializeField]
        private ETypeOfIMU mIMUSensorType;

        private Vector3 mEulerRawData;
        public Vector3 EulerRawData
        {
            get
            {
                return mEulerRawData;
            }
        }

        private float mVehicleSpeed;
        public float VehicleSpeed
        {
            get
            {
                return mVehicleSpeed;
            }
        }

        void Start()
        {
            KUsystem.Manager.Managers.Serial.OpenSerialPort();
        }

        void Update()
        {
            // Serial Data�� ���������� ������Ʈ�Ͽ� ������
            KUsystem.Manager.Managers.Serial.UpdateSerialData();

            string serialData = KUsystem.Manager.Managers.Serial.DataReceived;
            
            if(serialData == null)
            {
                return;
            }

            string[] splitedData = serialData.Split(',');

            // ROLL, PITCH, YAW, SPEED
            if (mIMUSensorType == ETypeOfIMU.OBD2)
            {
                if((splitedData.Length == 4)
                    && (float.TryParse(splitedData[0], out float x)))
                {
                    mEulerRawData.x = float.Parse(splitedData[0]);
                    mEulerRawData.z = float.Parse(splitedData[1]);
                    mEulerRawData.y = -float.Parse(splitedData[2]);
                    mVehicleSpeed = float.Parse(splitedData[3]);
                }
            }

            // ROLL, PITCH, YAW
            if(mIMUSensorType == ETypeOfIMU.BNO080)
            {
                // Z, X, Y => O
                if ((splitedData.Length == 3)
                    && (float.TryParse(splitedData[0], out float x)))
                {
                    mEulerRawData.x = float.Parse(splitedData[0]);
                    mEulerRawData.z = float.Parse(splitedData[1]);
                    mEulerRawData.y = -float.Parse(splitedData[2]);
                }
            }
        }

        void OnDestroy()
        {
            KUsystem.Manager.Managers.Serial.CloseSerialPort();
        }
    }

}

