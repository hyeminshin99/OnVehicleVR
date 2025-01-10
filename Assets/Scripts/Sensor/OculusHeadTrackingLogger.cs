using System;
using System.IO;
using UnityEngine;

public class OculusHeadTrackingLogger : MonoBehaviour
{
    [Header("User-defined Information")]
    [SerializeField] private string participantName; // 사용자 PA 이름
    [SerializeField] private string conditionName;   // Condition 이름

    private StreamWriter writer;
    private string filePath;

    // Start is called before the first frame update
    void Start()
    {
        // 로그를 저장할 디렉토리 경로
        string directoryPath = Path.Combine(Application.dataPath, "Logs");

        // 디렉토리가 없으면 생성
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log($"디렉토리가 생성되었습니다: {directoryPath}");
        }

        // 현재 날짜와 시간을 기반으로 파일 이름 생성 (PA와 Condition 포함)
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"{participantName}_{conditionName}_{timestamp}.csv";
        filePath = Path.Combine(directoryPath, fileName);

        // CSV 파일 생성 및 헤더 작성
        writer = new StreamWriter(filePath, false);
        writer.WriteLine("Time,PositionX,PositionY,PositionZ,RotationX,RotationY,RotationZ,RotationW");

        Debug.Log($"CSV 파일이 생성되었습니다: {filePath}");
    }

    // Update is called once per frame
    void Update()
    {
        if (writer != null)
        {
            // 현재 시간
            float time = Time.time;

            // 헤드셋의 위치와 회전 정보
            Vector3 position = Camera.main.transform.position;
            Quaternion rotation = Camera.main.transform.rotation;

            // 데이터를 CSV 형식으로 저장
            string line = $"{time},{position.x},{position.y},{position.z},{rotation.x},{rotation.y},{rotation.z},{rotation.w}";
            writer.WriteLine(line);
        }
    }

    // OnDestroy는 객체가 삭제되기 직전에 호출됩니다
    void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
            Debug.Log($"CSV 파일 저장이 완료되었습니다: {filePath}");
        }
    }
}
