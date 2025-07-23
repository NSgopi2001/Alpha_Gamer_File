using TMPro;
using UnityEngine;

public class ScoreUIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI highScoreText;

    void Update()
    {
        if (ScoreManager.Instance == null) return;

        scoreText.text = $"Score: {ScoreManager.Instance.Score}";
        movesText.text = $"Moves: {ScoreManager.Instance.Moves}";
        comboText.text = $"Combo: x{ScoreManager.Instance.ComboMultiplier}";
        highScoreText.text = $"High Score: {ScoreManager.Instance.HighScore}";
    }
}
