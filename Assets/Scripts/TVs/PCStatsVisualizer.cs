using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    private PCStatsData statsData;

    private string json = "{\"CPUUsage\": 31.2, \"RAMUsage\": 78.1, \"DiskUsage\": 64.8}";

    private void Start()
    {
        UpdateUIWithJSONData();
        InvokeRepeating("SimulateDataUpdate", 2f, 2f); // Simulate data update every 2 seconds
    }

    private void SimulateDataUpdate()
    {
        // Simulate minor changes in the JSON data
        // For the sake of demonstration, let's simulate some changes
        float cpuUsage = Random.Range(20f, 25f);
        float ramUsage = Random.Range(77f, 79f);
        float diskUsage = Random.Range(94f, 95f);

        json = "{\"CPUUsage\": \"" + cpuUsage.ToString("F1") + "%\", \"RAMUsage\": \"" + ramUsage.ToString("F1") + "%\", \"DiskUsage\": \"" + diskUsage.ToString("F1") + "%\"}";

        UpdateUIWithJSONData();
    }

    private void UpdateUIWithJSONData()
    {
        if (json != null)
        {
            statsData = JsonUtility.FromJson<PCStatsData>(json);

            if (statsData != null)
            {
                cpuUsageText.text = "Uso de CPU: " + statsData.CPUUsage.ToString("F1") + "%";
                ramUsageText.text = "Uso de RAM: " + statsData.RAMUsage.ToString("F1") + "%";
                diskUsageText.text = "Uso de disco: " + statsData.DiskUsage.ToString("F1") + "%";

                // Get RectTransform from the image components
                RectTransform cpuRectTransform = cpuImage.GetComponent<RectTransform>();
                RectTransform ramRectTransform = ramImage.GetComponent<RectTransform>();
                RectTransform diskRectTransform = diskImage.GetComponent<RectTransform>();

                if (cpuRectTransform != null)
                    cpuRectTransform.offsetMax = new Vector2(800 * (statsData.CPUUsage / 100f), cpuRectTransform.offsetMax.y);

                if (ramRectTransform != null)
                    ramRectTransform.offsetMax = new Vector2(800 * (statsData.RAMUsage / 100f), ramRectTransform.offsetMax.y);

                if (diskRectTransform != null)
                    diskRectTransform.offsetMax = new Vector2(800 * (statsData.DiskUsage / 100f), diskRectTransform.offsetMax.y);
            }
        }
    }
}
