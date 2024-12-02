using UnityEngine;
using System.IO.Ports;

public class SpeedManager : MonoBehaviour
{
    public static SpeedManager Instance { get; private set; }

    private SerialPort serialPort;
    public float Speed { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �Ŵ����� �ٸ� �������� ����
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        string portName = "COM4"; // ���� ��Ʈ �̸����� ����
        int baudRate = 9600;

        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
    }

    private void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                if (float.TryParse(data, out float speed))
                {
                    Speed = speed;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error reading VAG KKL data: " + e.Message);
            }
        }
    }

    private void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
