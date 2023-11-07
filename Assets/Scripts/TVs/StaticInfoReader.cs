using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class StaticInfoReader : MonoBehaviour
{
    public GameObject staticInfoPanel;
    public GameObject statsPanel;
    public TextMeshProUGUI displayText;
    private List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
    private int currentIndex = 0;
    private bool isStaticPanelEnabled = false;
    private float inputCooldown = 0.2f;
    private float lastInputTime;

    void Start()
    {
        LoadDataFromCSV();
    }

    void OnEnable()
    {
        DisplayCurrentPC();
        staticInfoPanel.SetActive(false);
        statsPanel.SetActive(true);
        isStaticPanelEnabled = false;
    }

    void LoadDataFromCSV()
    {
        string filePath = "Assets/Resources/StaticPCInfo.csv";
        string[] lines = File.ReadAllLines(filePath);

        string[] headers = lines[0].Split(',');
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            Dictionary<string, string> entry = new Dictionary<string, string>();
            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                entry[headers[j]] = values[j];
            }
            data.Add(entry);
        }
    }

    void DisplayCurrentPC()
    {
        Dictionary<string, string> currentPC = data[currentIndex];
        displayText.text = $"<b>PC {currentIndex}\n</b>";
        foreach (var kvp in currentPC)
        {
            displayText.text += "<b>" + kvp.Key + ":</b> " + kvp.Value + "\n";
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Time.time - lastInputTime > inputCooldown)
        {
            if (horizontalInput > 0.8f)
            {
                if (!isStaticPanelEnabled)
                {
                    statsPanel.SetActive(false);
                    staticInfoPanel.SetActive(true);
                    isStaticPanelEnabled = true;
                }

                currentIndex = (currentIndex + 1) % data.Count;
                DisplayCurrentPC();
                lastInputTime = Time.time;
            }
            else if (horizontalInput < -0.8f)
            {
                if (isStaticPanelEnabled)
                {
                    statsPanel.SetActive(false);
                    staticInfoPanel.SetActive(true);
                    isStaticPanelEnabled = true;
                }

                currentIndex = (currentIndex - 1 + data.Count) % data.Count;
                DisplayCurrentPC();
                lastInputTime = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            staticInfoPanel.SetActive(false);
            statsPanel.SetActive(true);
            isStaticPanelEnabled = false;
        }
    }
}
