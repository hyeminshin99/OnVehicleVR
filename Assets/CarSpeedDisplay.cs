using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class CarSpeedDisplay : MonoBehaviour
{
    SerialPort serialPort;
    public string portName = "COM4";  // ��Ʈ �̸�
    public int baudRate = 9600;
    public Text speedText;            // Canvas�� �ִ� Text ������Ʈ�� ����

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
            serialPort.ReadTimeout = 100;
            RequestSpeedData();  // �ӵ� ��û
        }
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                float speed = ParseSpeedData(data);
                UpdateSpeedDisplay(speed);  // �ӵ� ǥ�� ������Ʈ
            }
            catch (System.Exception)
            {
                // ��� ���� ó��
            }
        }
    }

    void RequestSpeedData()
    {
        if (serialPort.IsOpen)
        {
            serialPort.WriteLine("010D");  // �ӵ� ��û �ڵ�
        }
    }

    float ParseSpeedData(string data)
    {
        // ��: "41 0D 3C" ������ ������ �Ľ�
        if (data.StartsWith("41 0D"))
        {
            string hexSpeed = data.Substring(6, 2); // �ӵ� �� �κ� ����
            if (int.TryParse(hexSpeed, System.Globalization.NumberStyles.HexNumber, null, out int speed))
            {
                return speed;
            }
        }
        return 0f;
    }

    void UpdateSpeedDisplay(float speed)
    {
        if (speedText != null)
        {
            speedText.text = "Speed: " + speed + " km/h";
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
