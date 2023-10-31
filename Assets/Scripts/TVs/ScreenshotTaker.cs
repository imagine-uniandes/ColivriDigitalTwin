using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;

public class ScreenshotTaker : MonoBehaviour
{
    private string serverURL = "SERVER_URL"; // TODO: Replace with server URL
    private float interval = 900f; // 15 minutes in seconds

    private void Start()
    {
        InvokeRepeating("TakeAndSendScreenshots", 0f, interval);
    }

    public void TakeAndSendScreenshots()
    {
        StartCoroutine(CaptureAndSendScreenshots());
    }

    private IEnumerator CaptureAndSendScreenshots()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display disp = Display.displays[i];
            RenderTexture renderTexture = new RenderTexture(disp.systemWidth, disp.systemHeight, 24);
            Texture2D screenshot = new Texture2D(disp.systemWidth, disp.systemHeight, TextureFormat.RGB24, false);
            disp.Activate();
            ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
            RenderTexture.active = renderTexture;
            screenshot.ReadPixels(new Rect(0, 0, disp.systemWidth, disp.systemHeight), 0, 0);
            screenshot.Apply();
            byte[] bytes = screenshot.EncodeToPNG();

            StartCoroutine(PostScreenshot(bytes));
        }
    }

    private IEnumerator PostScreenshot(byte[] imageBytes)
    {
        UnityWebRequest www = UnityWebRequest.PostWwwForm(serverURL, "");
        www.uploadHandler = new UploadHandlerRaw(imageBytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "image/png");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Screenshot sent successfully!");
        }
    }
}
