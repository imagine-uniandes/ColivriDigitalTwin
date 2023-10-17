using UnityEngine;
using TMPro;

public class PCStatsVisualizer : MonoBehaviour
{
    public TextMeshProUGUI cpuUsageText;
    public TextMeshProUGUI ramUsageText;
    public TextMeshProUGUI diskUsageText;

    [System.Serializable]
    public class PCStatsData
    {
        public string CPUUsage;
        public string RAMUsage;
        public string DiskUsage;
    }

    private PCStatsData statsData;

    private string json = "{\"CPUUsage\": \"21.2%\", \"RAMUsage\": \"78.1%\", \"DiskUsage\": \"94.8%\"}";

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
                cpuUsageText.text = "Uso de CPU: " + statsData.CPUUsage;
                ramUsageText.text = "Uso de RAM: " + statsData.RAMUsage;
                diskUsageText.text = "Uso de disco: " + statsData.DiskUsage;

                // TODO: graph
            }
        }
    }
}
