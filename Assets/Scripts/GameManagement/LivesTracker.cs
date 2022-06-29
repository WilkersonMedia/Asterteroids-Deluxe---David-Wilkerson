using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameManager))]
public class LivesTracker : MonoBehaviour
{
    [SerializeField] public int Lives { get; private set; } = 2;
    [SerializeField] private GameObject livesIndicator = null;

    private GameObject[] livesOnScreen;

    private void Start()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("AsteroidsDeluxe")) return;
        DrawOnScreen();
    }
    private void DrawOnScreen()
    {
        if (Lives <= 0) return;
        livesOnScreen = new GameObject[Lives];
        for (var i = 0; i < Lives; i++)
        {
            float newX = (-4.7f + (0.4f * i));
            GameObject newLife = Instantiate(livesIndicator, new Vector3(newX, 4.2f, 0), transform.rotation, null);
            livesOnScreen[i] = newLife;
        }
    }
    private void ClearLivesOnScreen()
    {
        foreach (GameObject life in livesOnScreen)
        {
            Destroy(life);
        }
    }
    public void AddLife(int amount)
    {
        Mathf.Clamp(Lives += amount, -1, 10);
        if (Lives < 0) return;
        ClearLivesOnScreen();
        DrawOnScreen();
    }
}
