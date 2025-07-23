using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite bgSprite;

    public CardScriptableObject[] cardSOArray;

    public List<CardScriptableObject> cardSOList = new List<CardScriptableObject>();

    public List<Button> cardsList = new List<Button>();

    private void Awake()
    {
        cardSOArray = Resources.LoadAll<CardScriptableObject>("CardSO");  
    }

    void Start()
    {
        LoadAllCards();

        AddListeners();

        AddCardData();
    }

    private void LoadAllCards()
    {
        GameObject[] cardsArray = GameObject.FindGameObjectsWithTag("Card");

        if (cardsArray.Length == 0)
        {
            Debug.LogWarning("No cards found with tag 'Card'. Check if your prefabs are tagged properly.");
            return;
        }

        foreach (GameObject cards in cardsArray)
        {
            Button btn = cards.GetComponent<Button>();
            if (btn != null)
            {
                btn.image.sprite = bgSprite;
                cardsList.Add(btn);
            }
            else
            {
                Debug.LogWarning($"GameObject {cards.name} is tagged 'Card' but has no Button component.");
            }
        }

        Debug.Log($"Loaded {cardsList.Count} cards.");
    }

    private void AddCardData()
    {
        int looper = cardsList.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if(index == looper/2)
            {
                index = 0;
            }
            cardSOList.Add(cardSOArray[index]);
            index++;
        }

    }

    private void AddListeners()
    {
        foreach (Button cardButtons in cardsList)
        {
            cardButtons.onClick.AddListener(() => PickCard());
        }
    }

    private void PickCard()
    {
        string cardName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        Debug.Log(cardName + "Button pressed");
    }
}
