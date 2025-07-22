using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<Button> cardsList = new List<Button>();

    void Start()
    {
        LoadAllCards();

        AddListeners();
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
                cardsList.Add(btn);
            }
            else
            {
                Debug.LogWarning($"GameObject {cards.name} is tagged 'Card' but has no Button component.");
            }
        }

        Debug.Log($"Loaded {cardsList.Count} cards.");
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
