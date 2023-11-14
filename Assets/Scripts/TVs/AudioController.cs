using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class AudioController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction rotationAction;
    private ButtonNavigator buttonNavigator;
    public GameObject buttonPanel;
    public GameObject audiosGroup;

    private Button[] buttons;
    private List<GameObject> audioObjects = new List<GameObject>();

    private void OnEnable()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found. Please attach the PlayerInput component to this GameObject.");
        }
        rotationAction = playerInput.actions["Rotation"];

        buttonNavigator = GetComponentInParent<ButtonNavigator>();
        if (buttonNavigator == null)
            Debug.LogError("ButtonNavigator script not found in the parent GameObject.");
        buttonNavigator.SetButtons(buttonPanel.GetComponentsInChildren<Button>());
    }

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

    private void Update()
    {
        buttonNavigator.NavigateButtons(rotationAction.ReadValue<Vector3>().z);
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
