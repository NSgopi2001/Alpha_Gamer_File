using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public GridEnum.GridSize SelectedGridSize = GridEnum.GridSize.Grid_2x2;

    public List<CardSaveData> LastSavedCards = new List<CardSaveData>();

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

    [System.Serializable]
    public class CardSaveData
    {
        public string cardName;
        public bool isMatched;
    }

    [System.Serializable]
    public class GameSaveData
    {
        public List<CardSaveData> cards = new List<CardSaveData>();
    }

    public void SaveGame(List<Card> cards)
    {
        GameSaveData saveData = new GameSaveData();

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
        LastSavedCards = saveData.cards;

        Debug.Log($"Game saved to {savePath}");
    }

    public List<CardSaveData> LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found.");
            return null;
        }

        string json = File.ReadAllText(savePath);
        GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(json);
        LastSavedCards = loadedData.cards;

        Debug.Log("Game loaded from file.");
        return LastSavedCards;
    }

    public bool HasSaveData()
    {
        return File.Exists(savePath);
    }
}
