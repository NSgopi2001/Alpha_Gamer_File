using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public GridEnum.GridSize SelectedGridSize = GridEnum.GridSize.Grid_2x2;

    public List<CardSaveData> LastSavedCards = new List<CardSaveData>();

    public static bool hasContinued = false;

    private string savePath => Path.Combine(Application.persistentDataPath, "game_save.json");

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called from UI with int index
    public void SetGridSizeByIndex(int index)
    {
        SelectedGridSize = (GridEnum.GridSize)index;
    }

    public void LoadGridFile()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(json);
                SelectedGridSize = loadedData.gridSize;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load grid file: {ex.Message}");
        }
    }

    [System.Serializable]
    public class CardSaveData
    {
        public string cardName;
        public bool isMatched;
    }

    [System.Serializable]
    public class GameSaveData
    {
        public GridEnum.GridSize gridSize;
        public List<CardSaveData> cards = new List<CardSaveData>();
    }


    /// <summary>
    /// Saves grid size and card data to persistent path.
    /// </summary>
    public void SaveGame(List<Card> cards)
    {
        GameSaveData saveData = new GameSaveData
        {
            gridSize = SelectedGridSize
        };

        foreach (Card card in cards)
        {
            CardSaveData cardData = new CardSaveData
            {
                cardName = card.cardSO.cardName,
                isMatched = card.IsMatched
            };
            saveData.cards.Add(cardData);
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"Game saved to {savePath}");
    }

    /// <summary>
    /// Loads saved grid size and card data from persistent path.
    /// </summary>
    public GameSaveData LoadGame()
    {
        try
        {
            if (!File.Exists(savePath))
            {
                Debug.LogWarning("No save file found.");
                return null;
            }

            string json = File.ReadAllText(savePath);
            GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(json);
            SelectedGridSize = loadedData.gridSize;
            Debug.Log("Game loaded from file.");
            return loadedData;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading game data: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Check if save file exists.
    /// </summary>
    public bool HasSaveData()
    {
        return File.Exists(savePath);
    }

    /// <summary>
    /// Delete the saved file (optional utility).
    /// </summary>
    public void ClearSaveData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
    }

    public void SetContinueBool(bool value)
    {
        hasContinued = value;
    }

    public bool GetContinueBool()
    {
        return hasContinued;
    }
}
