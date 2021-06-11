using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    // Game Variables
    public float timeSpent;
    public float levelStartTime;
    public int munitionLeft;
    public float health;

    // Points Variables
    public int points;

    // Ranking Variables
    public int[] highScores = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public string[] highScorePlayers = new string[10] { "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe" };
    public string playerName;

    private static GameManager _instance;

    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameManager();
        }

        return _instance;
    }

    private GameManager()
    {
        this.points = 0;
        this.levelStartTime = 0;
        this.munitionLeft = 20;
        this.health = 100;
        this.playerName = "Jane Doe";
    }

    public void Reset()
    {
        UpdateHighScore();
        this.points = 0;
        this.munitionLeft = 20;
        this.health = 100;
        this.playerName = "Jane Doe";
    }

    public void DamagePlayer(float amount = 10.0f)
    {
        this.health -= amount;

        if (this.health <= 0)
        {
            SceneManager.LoadScene("YouDied");
        }
    }

    public void StartLevel()
    {
        this.levelStartTime = Time.time;
    }

    public void AddMunition()
    {
        this.munitionLeft += 20;
    }

    public void AddPoints(int points)
    {
        this.points += points;
    }

    private void UpdateHighScore()
    {
        int i;
        for (i = 0; i <= 9; i++)
        {
            if (points > highScores[i]) break;
        }
        if (i < 10)
        {
            for (int j = 9; j > i; j--)
            {
                highScorePlayers[j] = highScorePlayers[j - 1];
                highScores[j] = highScores[j - 1];
            }
            highScorePlayers[i] = playerName;
            highScores[i] = points;
        }
    }
}
