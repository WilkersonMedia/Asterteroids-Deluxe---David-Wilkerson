using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(GameManager))]
public class PointsTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTextElement;
    [SerializeField] TextMeshProUGUI bonusTextElement;
    [SerializeField] TextMeshProUGUI highScoreTextElement;
    [SerializeField] private int scoreForBonusLife = 10000;

    private int nextBonusLife = 0;
    private static int highScore = 0;
    private int totalPoints;

    private void Start() => InstantiateData();

    private void InstantiateData()
    {
        highScore = PlayerPrefs.GetInt("highscore");
        highScoreTextElement.text = highScore.ToString();
        scoreTextElement.text = GetPoints().ToString();
        UpdateBonusLife(false);
    }
    public void AddPoints(int points)
    {
        totalPoints += points;
        scoreTextElement.text = GetPoints().ToString();
        CheckForHighScore();
        if (GetPoints() < nextBonusLife) return;
        gameObject.GetComponent<LivesTracker>().AddLife(1);
        UpdateBonusLife(true);
    }
    private void UpdateBonusLife(bool shouldPlayAudio)
    {
        nextBonusLife += scoreForBonusLife;
        bonusTextElement.text = "Bonus At " + nextBonusLife;
        if (shouldPlayAudio) this.GetComponent<AudioManager>().Play1UP();
    }
    public int GetPoints()
    {
        return totalPoints;
    }
    private int CheckForHighScore()
    {
        if (totalPoints > highScore)
        {
            highScore = totalPoints;
            highScoreTextElement.text = highScore.ToString();
            return totalPoints;
        }
        else return 0;
    }
    public bool IsNewHighScore()
    {
        if (highScore == totalPoints)
        {
            PlayerPrefs.SetInt("highscore", highScore);
            return true;
        }
        else return false;
    }
}
