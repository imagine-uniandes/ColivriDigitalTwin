using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeaderPanelNavigation : MonoBehaviour
{
    public Button[] headerButtons;
    private int selectedIndex = 0;
    public GameObject homePanel;
    [SerializeField] private GameObject[] panelsToHide;

    private bool isHeld = false;
    private float holdTimer = 0f;
    private float holdDuration = 3f; // 3 seconds hold duration for reset

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            headerButtons[selectedIndex].Select();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            selectedIndex = Mathf.Min(headerButtons.Length - 1, selectedIndex + 1);
            headerButtons[selectedIndex].Select();
        }

        if (Input.GetKey(KeyCode.H))
        {
            isHeld = true;
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
            {
                ResetGame();
            }
        }
        else
        {
            if (isHeld)
            {
                isHeld = false;
                if (holdTimer < holdDuration)
                {
                    HomeAction();
                }
                holdTimer = 0f;
            }
        }
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HomeAction()
    {
        if (homePanel != null)
        {
            homePanel.SetActive(true);
            // Hide all other panels
            foreach (var panel in panelsToHide)
            {
                if (panel != null)
                    panel.SetActive(false);
            }
        }
    }
}
