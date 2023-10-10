using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioController : MonoBehaviour
{
    public GameObject buttonPanel; // Reference to the ButtonPanel group
    public GameObject audiosGroup; // Reference to the Audios group

    private Button[] buttons;
    private List<GameObject> audioObjects = new List<GameObject>(); // Use a list to store audio objects

    private void Start()
    {
        // Get all the buttons in the button panel
        buttons = buttonPanel.GetComponentsInChildren<Button>();

        // Find all audio objects under the audios group and add them to the list
        foreach (Transform child in audiosGroup.transform)
        {
            audioObjects.Add(child.gameObject);
            child.gameObject.SetActive(false); // Disable all audio objects initially
        }

        // Attach OnClick listeners to each button
        for (int i = 0; i < buttons.Length; i++)
        {
            int audioIndex = i; // Capture the index in a local variable for the listener
            buttons[i].onClick.AddListener(() => ActivateAudio(audioIndex));
        }
    }

    public void ActivateAudio(int audioIndex)
    {
        // Deactivate all audio objects in the Audios group
        foreach (GameObject audioObject in audioObjects)
        {
            audioObject.SetActive(false);
        }

        // Activate the selected audio object based on the index
        if (audioIndex >= 0 && audioIndex < audioObjects.Count)
        {
            audioObjects[audioIndex].SetActive(true);
        }
    }
}
