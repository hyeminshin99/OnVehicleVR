using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class CarSpeedDisplay : MonoBehaviour
{
    SerialPort serialPort;
    public string portName = "COM4";  // 포트 이름
    public int baudRate = 9600;
    public Text speedText;            // Canvas에 있는 Text 컴포넌트를 연결

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
            serialPort.ReadTimeout = 100;
            RequestSpeedData();  // 속도 요청
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
                UpdateSpeedDisplay(speed);  // 속도 표시 업데이트
            }
            catch (System.Exception)
            {
                // 통신 오류 처리
            }
        }
    }

    void RequestSpeedData()
    {
        if (serialPort.IsOpen)
        {
            serialPort.WriteLine("010D");  // 속도 요청 코드
        }
    }

    float ParseSpeedData(string data)
    {
        // 예: "41 0D 3C" 형식의 데이터 파싱
        if (data.StartsWith("41 0D"))
        {
            string hexSpeed = data.Substring(6, 2); // 속도 값 부분 추출
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
