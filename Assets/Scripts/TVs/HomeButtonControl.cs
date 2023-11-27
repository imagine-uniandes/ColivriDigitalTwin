using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.EventSystems;

public class HomeButtonControl : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction rotationAction;
    public Button resetButton;
    public Button exitButton;
    public GameObject buttonsGroup;
    private ButtonNavigator buttonNavigator;

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
        buttonNavigator.SetButtons(buttonsGroup.GetComponentsInChildren<Button>());
    }

    private void Start()
    {
        resetButton.onClick.AddListener(ResetGame);
        exitButton.onClick.AddListener(ExitGame);
        EventSystem.current.SetSelectedGameObject(buttonsGroup.GetComponentsInChildren<Button>()[0].gameObject);
    }

    private void Update()
    {
        buttonNavigator.NavigateButtons(rotationAction.ReadValue<Vector3>().y);
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
