using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PCStatsVisualizer : MonoBehaviour
{
    public TextMeshProUGUI cpuUsageText;
    public TextMeshProUGUI ramUsageText;
    public TextMeshProUGUI diskUsageText;

    public Image cpuImage;
    public Image ramImage;
    public Image diskImage;

    [System.Serializable]
    public class PCStatsData
    {
        public float CPUUsage;
        public float RAMUsage;
        public float DiskUsage;
    }

    private List<PCStatsData> allStatsData = new List<PCStatsData>();

    private string baseURL = "http://18.188.1.225:8080/data/";

    private void Start()
    {
        StartCoroutine(GetAllData());
        InvokeRepeating("GetAllData", 30f, 30f);
    }

    private IEnumerator GetAllData()
    {
        for (int i = 1; i <= 24; i++)
        {
            UnityWebRequest www = UnityWebRequest.Get(baseURL + i);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                PCStatsData statsData = JsonUtility.FromJson<PCStatsData>(json);
                allStatsData.Add(statsData);
            }

            CalculateAndDisplayAverage();
        }

        void CalculateAndDisplayAverage()
        {
            if (allStatsData.Count > 0)
            {
                float avgCPU = 0f;
                float avgRAM = 0f;
                float avgDisk = 0f;

                foreach (var data in allStatsData)
                {
                    avgCPU += data.CPUUsage;
                    avgRAM += data.RAMUsage;
                    avgDisk += data.DiskUsage;
                }

                avgCPU /= allStatsData.Count;
                avgRAM /= allStatsData.Count;
                avgDisk /= allStatsData.Count;

                PCStatsData avgData = new PCStatsData
                {
                    CPUUsage = avgCPU,
                    RAMUsage = avgRAM,
                    DiskUsage = avgDisk
                };

                UpdateUIWithJSONData(avgData);
            }
        }

        void UpdateUIWithJSONData(PCStatsData data)
        {
            if (data != null)
            {
                cpuUsageText.text = "Uso de CPU: " + data.CPUUsage.ToString("F1") + "%";
                ramUsageText.text = "Uso de RAM: " + data.RAMUsage.ToString("F1") + "%";
                diskUsageText.text = "Uso de disco: " + data.DiskUsage.ToString("F1") + "%";

                // Get RectTransform from the image components
                RectTransform cpuRectTransform = cpuImage.GetComponent<RectTransform>();
                RectTransform ramRectTransform = ramImage.GetComponent<RectTransform>();
                RectTransform diskRectTransform = diskImage.GetComponent<RectTransform>();

                if (cpuRectTransform != null)
                    cpuRectTransform.offsetMax = new Vector2(800 * (data.CPUUsage / 100f), cpuRectTransform.offsetMax.y);

                if (ramRectTransform != null)
                    ramRectTransform.offsetMax = new Vector2(800 * (data.RAMUsage / 100f), ramRectTransform.offsetMax.y);

                if (diskRectTransform != null)
                    diskRectTransform.offsetMax = new Vector2(800 * (data.DiskUsage / 100f), diskRectTransform.offsetMax.y);
            }
        }
    }
}
