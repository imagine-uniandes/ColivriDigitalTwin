using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.EventSystems;

public class StaticInfoReader : MonoBehaviour
{
    public GameObject staticInfoPanel;
    public GameObject statsPanel;
    public TextMeshProUGUI displayText;
    private List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
    private int currentIndex = 0;
    private bool isStaticPanelEnabled = false;
    private float inputCooldown = 0.2f;
    public float rotationThreshold = 0.25f;
    private float lastInputTime;
    private PlayerInput playerInput;
    private InputAction rotationAction;

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

        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found. Please attach the PlayerInput component to this GameObject.");
        }
        rotationAction = playerInput.actions["Rotation"];
    }

    void LoadDataFromCSV()
    {
        TextAsset textAsset = Resources.Load("StaticPCInfo") as TextAsset;
        string[] lines = textAsset.text.Split('\n');

        string[] headers = lines[0].Split(',');
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Trim().Split(',');

            // Check if the line is not empty
            if (values.Length > 0 && !string.IsNullOrEmpty(values[0]))
            {
                Dictionary<string, string> entry = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    entry[headers[j]] = values[j];
                }
                dataList.Add(entry);
            }
        }
    }

    void DisplayCurrentPC()
    {
        // Check if dataList is not empty and currentIndex is a valid index
        if (dataList.Count > 0 && currentIndex >= 0 && currentIndex < dataList.Count)
        {
            Dictionary<string, string> currentPC = dataList[currentIndex];
            displayText.text = $"<b>PC {currentIndex}</b>\n";

            // Iterate over key-value pairs, excluding the last one
            int count = 0;
            foreach (var kvp in currentPC)
            {
                if (count < currentPC.Count - 1)
                {
                    displayText.text += "<b>" + kvp.Key + ":</b> " + kvp.Value + "\n";
                }
                count++;
            }
        }
        else
        {
            displayText.text = "No data available";
        }
    }

    void Update()
    {
        // Change index by using right/left movement
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Time.time - lastInputTime > inputCooldown)
        {
            if (horizontalInput > 0.5f)
            {
                HandleHorizontalInputPositive();
            }
            else if (horizontalInput < -0.5f)
            {
                HandleHorizontalInputNegative();
            }
        }

        // Change index by using rotation
        RotateSpace(rotationAction.ReadValue<Vector3>().y);


        if (Input.GetKeyDown(KeyCode.Return))
        {
            staticInfoPanel.SetActive(false);
            statsPanel.SetActive(true);
            isStaticPanelEnabled = false;
        }
    }

    public void RotateSpace(float rotation)
    {
        if (Mathf.Abs(rotation) > rotationThreshold && Time.time - lastInputTime >= inputCooldown)
        {
            // Move to the next or previous button based on the rotation direction
            if (rotation > rotationThreshold)
            {
                HandleHorizontalInputPositive();
            }
            else if (rotation < -rotationThreshold)
            {
                HandleHorizontalInputNegative();
            }

            lastInputTime = Time.time;
        }
    }

    void HandleHorizontalInputPositive()
    {
        if (!isStaticPanelEnabled)
        {
            statsPanel.SetActive(false);
            staticInfoPanel.SetActive(true);
            isStaticPanelEnabled = true;
        }

        currentIndex = (currentIndex + 1) % dataList.Count;
        DisplayCurrentPC();
        lastInputTime = Time.time;
    }

    void HandleHorizontalInputNegative()
    {
        if (!isStaticPanelEnabled)
        {
            statsPanel.SetActive(false);
            staticInfoPanel.SetActive(true);
            isStaticPanelEnabled = true;
        }

        currentIndex = (currentIndex - 1 + dataList.Count) % dataList.Count;
        DisplayCurrentPC();
        lastInputTime = Time.time;
    }
}
