using UnityEngine;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score { get; private set; }
    public int Moves { get; private set; }
    public int ComboMultiplier { get; private set; }
    public int HighScore { get; private set; }

    private float comboResetTime = 3f;
    private float lastComboTime;

    private string scorePath => Path.Combine(Application.persistentDataPath, "score_save.json");
    private string highScorePath => Path.Combine(Application.persistentDataPath, "highscore_save.json");

    [System.Serializable]
    private class ScoreSaveData
    {
        public int score;
        public int moves;
    }

    [System.Serializable]
    private class HighScoreData
    {
        public int highScore;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        //LoadScoreData();
        LoadHighScore();
    }

    private void Start()
    {
        ComboMultiplier = 1;
    }

    void Update()
    {
        if (ComboMultiplier > 1 && Time.time - lastComboTime > comboResetTime)
        {
            ComboMultiplier = 1;
        }
    }

    public void ResetScore()
    {
        Score = 0;
        Moves = 0;
        ComboMultiplier = 1;
        //SaveScoreData();
    }

    public void AddMatchPoints(int basePoints)
    {
        if (basePoints < 0)
        {
            Score += basePoints;
        }
        else
        {
            Score += basePoints * ComboMultiplier;
            ComboMultiplier++;
        }

        if (Score < 0)
            Score = 0;

        lastComboTime = Time.time;
        UpdateHighScore();
        //SaveScoreData();
    }

    public void IncrementMoves()
    {
        Moves++;
        //SaveScoreData();
    }

    public void ResetCombo()
    {
        ComboMultiplier = 1;
    }

    private void UpdateHighScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
            SaveHighScore();
        }
    }

    public void SaveScoreData()
    {
        ScoreSaveData data = new ScoreSaveData
        {
            score = Score,
            moves = Moves
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(scorePath, json);
        Debug.Log("Score saved: " + json); // <-- Add this
    }


    public void LoadScoreData()
    {
        if (File.Exists(scorePath))
        {
            string json = File.ReadAllText(scorePath);
            ScoreSaveData data = JsonUtility.FromJson<ScoreSaveData>(json);
            SetScore(data.score, data.moves);  
        }
        else
        {
            SetScore(0, 0);
        }
    }


    private void SaveHighScore()
    {
        HighScoreData data = new HighScoreData { highScore = HighScore };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(highScorePath, json);
    }

    private void LoadHighScore()
    {
        if (File.Exists(highScorePath))
        {
            string json = File.ReadAllText(highScorePath);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            HighScore = data.highScore;
        }
        else
        {
            HighScore = 0;
        }
    }

    public void ClearAllData()
    {
        if (File.Exists(scorePath)) File.Delete(scorePath);
        if (File.Exists(highScorePath)) File.Delete(highScorePath);

        Score = 0;
        Moves = 0;
        HighScore = 0;
    }

    public void SetScore(int score, int moves)
    {
        Score = score;
        Moves = moves;
    }

}
