using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

    // Game Variables
    private int level;
    public float timeSpent;
    public float levelStartTime;
    public int munitionLeft;


    // Points Variables
    public int points;


    // Ranking Variables
    public int[] highScores = new int[10] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public string[] highScorePlayers = new string[10] {"Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe", "Jane Doe"};
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
        this.level = 1;
        this.points = 0;
        this.levelStartTime = 0;
        this.munitionLeft = 20;
        this.playerName = "Jane Doe";
    }

    public void Reset() {
        UpdateHighScore();
        this.level = 1;
        this.points = 0;
        this.munitionLeft = 20;
        this.playerName = "Jane Doe";
    }

    public void StartLevel() {
        this.levelStartTime = Time.time;
    }

    public void AddMunition() {
        this.munitionLeft += 20;
    }

    public void AddPoints(int points) {
        this.points += points;
    }

    private void UpdateHighScore() {
        int i;
        for (i = 0; i <= 9; i++) {
            if (points > highScores[i]) break;
        }
        if (i < 10) {
            for (int j = 9; j > i; j--) {
                highScorePlayers[j] = highScorePlayers[j-1];
                highScores[j] = highScores[j-1];
            }
            highScorePlayers[i] = playerName;
            highScores[i] = points;
        }
    }

}
