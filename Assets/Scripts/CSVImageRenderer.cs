using UnityEngine;
using System.IO;

public class CSVImageRenderer : MonoBehaviour
{
    public Texture2D RenderedImage;
    public string csvFilePath = "path/to/your/csv/file.csv";
    public int width = 256; // Change to match the width of your image
    public int height = 256; // Change to match the height of your image

    private void Start()
    {
        RenderImageFromCSV();
    }

    private void RenderImageFromCSV()
    {
        // Initialize the texture
        RenderedImage = new Texture2D(width, height);

        // Read the CSV file
        string[] csvLines = File.ReadAllLines(csvFilePath);

        // Ensure the CSV data has the expected number of rows
        if (csvLines.Length != height)
        {
            Debug.LogError("CSV file does not have the expected number of rows.");
            return;
        }

        for (int y = 0; y < height; y++)
        {
            string[] pixelValues = csvLines[y].Split(',');

            // Ensure the CSV data has the expected number of columns
            if (pixelValues.Length != width)
            {
                Debug.LogError("CSV file does not have the expected number of columns.");
                return;
            }

            for (int x = 0; x < width; x++)
            {
                Color pixelColor;
                if (TryParseColor(pixelValues[x], out pixelColor))
                {
                    RenderedImage.SetPixel(x, y, pixelColor);
                }
                else
                {
                    Debug.LogError($"Invalid color value in CSV at row {y}, column {x}: {pixelValues[x]}");
                }
            }
        }

        // Apply the changes to the texture
        RenderedImage.Apply();
    }

    private bool TryParseColor(string value, out Color color)
    {
        string[] components = value.Split(';');
        if (components.Length == 3 && float.TryParse(components[0], out float r) && float.TryParse(components[1], out float g) && float.TryParse(components[2], out float b))
        {
            color = new Color(r, g, b);
            return true;
        }
        color = Color.black;
        return false;
    }
}
