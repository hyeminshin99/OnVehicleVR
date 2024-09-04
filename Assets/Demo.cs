using Assets;
using Assets.Device.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    // Search List
    public GameObject deviceScanResultProto;
    Transform scanResultRoot;
    // Sensor Data List
    public GameObject deviceDataResultProto;
    Transform dataResultRoot;
    // Message text
    public Text TextMsg;

    // Device Service
    DeviceService deviceService;
    //Udp Service
    UdpServer udpServer;
    // List of device components
    List<GameObject> deviceModels = new List<GameObject>();
    // List of newly discovered devices
    List<DeviceModel> findList = new List<DeviceModel>();

    void Start()
    {
        // Bind UDP log events
        udpServer = WitApplication.Context.GetBean<UdpServer>();
        udpServer.msgEvent.AddListener(OnMsg);
        // Bind device search events
        deviceService = WitApplication.Context.GetBean<DeviceService>();
        deviceService.putDeviceEvent.AddListener(OnFindDevice);
        // Initialize List
        scanResultRoot = deviceScanResultProto.transform.parent;
        deviceScanResultProto.transform.SetParent(null);
        dataResultRoot = deviceDataResultProto.transform.parent;
        deviceDataResultProto.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        // If new devices are found, add them
        if (findList.Count > 0) {
            DeviceModel deviceModel = findList[0];
            GameObject g = Instantiate(deviceScanResultProto, scanResultRoot);
            g.name = deviceModel.DeivceId;
            g.transform.GetChild(0).GetComponent<Text>().text = deviceModel.DeivceId;
            findList.RemoveAt(0);

            GameObject d = Instantiate(deviceDataResultProto, dataResultRoot);
            d.name = deviceModel.DeivceId;
            d.transform.GetChild(0).GetComponent<Text>().text = deviceModel.DeivceId;
            d.transform.GetChild(1).GetComponent<Text>().text = GetDeviceData(deviceModel);
            deviceModels.Add(d);
        }

        // Update added device data
        for (int i = 0; i < deviceModels.Count; i++)
        {
            GameObject d = deviceModels[i];
            d.transform.GetChild(1).GetComponent<Text>().text = GetDeviceData(deviceService.GetDevice(d.name));
        }
    }

    /// <summary>
    /// Get device data
    /// </summary>
    private string GetDeviceData(DeviceModel deviceModel)
    {
        string Acc = $"AccX:{deviceModel.AccX}g\t\tAccY:{deviceModel.AccY}g\t\tAccZ:{deviceModel.AccZ}g\r\n";
        string As = $"AsX:{deviceModel.AsX}°/s\t\tAsY:{deviceModel.AsY}°/s\t\tAsZ:{deviceModel.AsZ}°/s\r\n";
        string Angle = $"AngleX:{deviceModel.AngleX}°\t\tAngleY:{deviceModel.AngleY}°\t\tAngleZ:{deviceModel.AngleZ}°\r\n";
        string Mag = $"HX:{deviceModel.HX}ut\t\tHY:{deviceModel.HY}ut\t\tHZ:{deviceModel.HZ}ut\r\n";
        string Electricity = $"Electricity:{deviceModel.Electricity}%";
        string data = Acc + As + Angle + Mag + Electricity;
        return data;
    }

    public Vector3 GetDeviceAngles(string deviceId)
    {
        DeviceModel deviceModel = deviceService.GetDevice(deviceId);
        if (deviceModel != null)
        {
            return new Vector3((float)deviceModel.AngleX, (float)deviceModel.AngleY, (float)deviceModel.AngleZ);
        }
        return Vector3.zero; // 디바이스가 없을 경우 기본값 반환
    }

    // 가장 최근에 발견된 디바이스 ID를 반환하는 메서드
    public string GetLatestDeviceId()
    {
        if (deviceModels.Count > 0)
        {
            // 가장 최근에 추가된 디바이스의 ID를 반환
            return deviceModels[deviceModels.Count - 1].name;
        }
        return null; // 디바이스가 없을 경우 null 반환
    }

    /// <summary>
    /// Open UDP service
    /// </summary>
    public void StartUDP() {
        try
        {
            udpServer.StartReceive();
        }
        catch (Exception ex)
        {
            MonoBehaviour.print(ex.Message);
        }
    }

    /// <summary>
    /// Broadcast sending local IP address
    /// </summary>
    public void SendLoc() {
        try
        {
            udpServer.SendLoc();
        }
        catch (Exception ex)
        {
            MonoBehaviour.print(ex.Message);
        }
    }

    /// <summary>
    /// Turn off UDP service
    /// </summary>
    public void StopUDP() {
        try
        {
            udpServer.StopReceive();
        }
        catch (Exception ex)
        {
            MonoBehaviour.print(ex.Message);
        }
    }

    /// <summary>
    /// This method will be executed when a new device is found
    /// </summary>
    /// <param name="deviceModel"></param>
    private void OnFindDevice(DeviceModel deviceModel) 
    {
        findList.Add(deviceModel);  
    }

    /// <summary>
    /// This method will be executed when updating the Message
    /// </summary>
    /// <param name="msg"></param>
    private void OnMsg(string msg) {
        try
        {
            TextMsg.text = msg;
        }
        catch (Exception)
        {
            return;
        }
    }
}
