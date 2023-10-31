using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeButtonControl : MonoBehaviour
{
    public Button resetButton;
    public Button exitButton;

    private void Start()
    {
        resetButton.onClick.AddListener(ResetGame);
        exitButton.onClick.AddListener(ExitGame);
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
