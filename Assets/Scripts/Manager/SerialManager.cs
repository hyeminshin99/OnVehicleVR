using System.IO.Ports;
using UnityEngine;
using UnityEngine.Assertions;

namespace SeungHoon.Manager
{
    // ��ǻ�� ȯ�濡 ���� Serial Port�� �ֱ������� Update ���־�� ��
    public class SerialManager
    {
        SerialPort stream = new SerialPort("COM5", 115200);

        private string mDataReceived;
        public string DataReceived
        {
            get
            {
                if(mDataReceived == null)
                {
                    return null;
                }

                return mDataReceived;
            }
        }

        public void CloseSerialPort()
        {
            Assert.IsTrue(stream.IsOpen);

            stream.Close();
        }

        public void OpenSerialPort()
        {
            stream.Open();
        }

        public void PrintSerialDataOnUnityConsole()
        {
            Assert.IsNotNull(mDataReceived);

            Debug.Log("Serial Data: " + mDataReceived);
        }

        public void UpdateSerialData()
        {
            mDataReceived = stream.ReadLine();
        }
    }
}

