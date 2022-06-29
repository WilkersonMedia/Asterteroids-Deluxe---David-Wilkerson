using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject bonusText;
    [SerializeField] TextMeshProUGUI gameoverText;
    [SerializeField] private string startMenuSceneName = "StartMenu";
    [SerializeField] private string mainGameSceneName = "AsteroidsDeluxe";
    private InputControls inputControls;
    private string currentScene;

    private void Start() => InitializeData();
    private void OnEnable() => SubscribeToControls();
    private void OnDisable() => UnSubscribeFromControls();

//Input Configuration
    private void EscPressed(InputAction.CallbackContext obj) => ChangeScene("Esc");
    private void EnterPressed(InputAction.CallbackContext obj) => ChangeScene("Enter");
    private void SubscribeToControls()
    {
        inputControls = new InputControls();
        inputControls.MenuMap.Select.performed += EnterPressed;
        inputControls.MenuMap.BackButton.performed += EscPressed;
        inputControls.Disable();
    }
    private void UnSubscribeFromControls()
    {
        inputControls.MenuMap.Select.performed -= EnterPressed;
        inputControls.MenuMap.BackButton.performed -= EscPressed;
        inputControls.Disable();
    }
//SceneManagement
    public void ChangeScene(string buttonPressed)
    {
        switch (currentScene)
        {
            case "StartMenu":
                if(buttonPressed == "Esc") Application.Quit();
                if(buttonPressed == "Enter") SceneManager.LoadScene(mainGameSceneName);
                break;
            case "AsteroidsDeluxe":
                if(buttonPressed == "Esc") SceneManager.LoadScene(0);
                if(buttonPressed == "Enter") SceneManager.LoadScene(startMenuSceneName);
                break;
            default:
                Debug.LogWarning("Current scene maybe named wrong. Current scene is listed as: " + currentScene.ToString());
                break;
        }
    }
//Other
    public void GameOver()
    {
        bonusText.SetActive(false);
        gameoverText.gameObject.SetActive(true);
        Invoke(nameof(DisplayPressEnterPrompt), 2);
        if (GetComponent<PointsTracker>().IsNewHighScore())
        {
            gameoverText.text = "New Highscore!!!";
        }
    }
    private void InitializeData()
    {
        currentScene = SceneManager.GetActiveScene().name.ToString();
        Debug.Log("Current Scene is: " + currentScene);
        if (currentScene == startMenuSceneName)
        {
            inputControls.Enable();
        }
    }
    private void DisplayPressEnterPrompt()
    {
        gameoverText.transform.GetChild(0).gameObject.SetActive(true);
        inputControls.Enable();
    }
}
