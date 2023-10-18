using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CameraPositionExporter : MonoBehaviour
{
    private float timer = 0f;
    private float interval = 5f;
    private string csvFilePath = "CameraPositions.csv";
    private List<string> positionData = new List<string>();

    void Update()
    {
        timer += Time.deltaTime;

        // Check if it's time to capture the position
        if (timer >= interval)
        {
            // Get the camera position
            Vector3 cameraPosition = transform.position;

            // Convert the position to a CSV-friendly format (e.g., comma-separated)
            string csvData = string.Format("{0},{1},{2}", cameraPosition.x, cameraPosition.y, cameraPosition.z);

            // Add the data to the list
            positionData.Add(csvData);

            // Reset the timer
            timer = 0f;
        }
    }

    void OnApplicationQuit()
    {
        // Define the full file path
        string fullPath = Path.Combine(Application.dataPath, csvFilePath);

        // Combine the CSV data into a single string
        string csvContent = string.Join("\n", positionData);

        // Check if the file exists
        if (!File.Exists(fullPath))
        {
            // Create the file if it doesn't exist and add a header row
            File.WriteAllText(fullPath, "X,Y,Z\n", Encoding.UTF8);
        }

        // Append the CSV data to the file
        File.AppendAllText(fullPath, csvContent);

        Debug.Log("CSV data appended to file at: " + fullPath);
    }
}