using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class HomeButtonControl : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction rotationAction;
    public Button resetButton;
    public Button exitButton;
    public GameObject buttonsGroup;

    private Button[] buttons;
    private int selectedIndex = 0;
    public float rotationThreshold = 0.25f;
    public float rotationCooldown = 0.25f;
    private float lastRotationTime;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found. Please attach the PlayerInput component to this GameObject.");
        }
        rotationAction = playerInput.actions["Rotation"];
    }

    private void Start()
    {
        buttons = buttonsGroup.GetComponentsInChildren<Button>();
        resetButton.onClick.AddListener(ResetGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void Update()
    {
        float rotationZ = rotationAction.ReadValue<Vector3>().z;

        if (Mathf.Abs(rotationZ) > rotationThreshold && Time.time - lastRotationTime >= rotationCooldown)
        {
            // Move to the next or previous button based on the rotation direction
            if (rotationZ > rotationThreshold)
            {
                MoveToNextButton();
            }
            else if (rotationZ < -rotationThreshold)
            {
                MoveToPreviousButton();
            }

            lastRotationTime = Time.time;
        }
    }

    private void MoveToNextButton()
    {
        selectedIndex = (selectedIndex + 1) % buttons.Length;
        buttons[selectedIndex].Select();
    }

    private void MoveToPreviousButton()
    {
        selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
        buttons[selectedIndex].Select();
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
