using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite bgSprite;

    public CardScriptableObject[] cardSOArray;

    public List<CardScriptableObject> cardSOList = new List<CardScriptableObject>();

    public List<Button> cardButtonList = new List<Button>();

    private bool firstGuess, secondGuess;

    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private int firstGuessIndex, secondGuessIndex;

    private string firstGuessName, secondGuessName;

    private void Awake()
    {
        cardSOArray = Resources.LoadAll<CardScriptableObject>("CardSO");  
    }

    void Start()
    {
        LoadAllCards();

        AddListeners();

        AddCardData();

        Shuffle(cardSOList);

        ApplyCardSprites(cardButtonList);

        gameGuesses = cardSOList.Count / 2;
    }

    private void ApplyCardSprites(List<Button> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            Card cardComponent = cardList[i].GetComponent<Card>();
            if (cardComponent != null)
            {
                cardComponent.SetCardSprite(cardSOList[i].cardSprite);
            }
            else
            {
                Debug.LogWarning($"No Card component found on {cardList[i].name}");
            }
        }
    }


    private void LoadAllCards()
    {
        GameObject[] cardsArray = GameObject.FindGameObjectsWithTag("Card");

        if (cardsArray.Length == 0)
        {
            Debug.LogWarning("No cards found with tag 'Card'. Check if your prefabs are tagged properly.");
            return;
        }

        foreach (GameObject card in cardsArray)
        {
            Button btn = card.GetComponent<Button>();
            if (btn != null)
            {
                btn.image.sprite = bgSprite;
                cardButtonList.Add(btn);
            }
            else
            {
                Debug.LogWarning($"GameObject {card.name} is tagged 'Card' but has no Button component.");
            }
        }

        Debug.Log($"Loaded {cardButtonList.Count} cards.");
    }

    private void AddCardData()
    {
        int looper = cardButtonList.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if(index == looper/2)
            {
                index = 0;
            }
            cardSOList.Add(cardSOArray[index]);
            //cardButtonList[i].GetComponent<Card>().cardSO = cardSOList[index];
            index++;
        }

    }

    private void AddListeners()
    {
        for (int i = 0; i < cardButtonList.Count; i++)
        {
            int index = i; 
            cardButtonList[i].onClick.AddListener(() => PickCard(index));
        }
    }


    private void PickCard(int index)
    {
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = index;
            firstGuessName = cardSOList[firstGuessIndex].cardName;

            cardButtonList[firstGuessIndex].interactable = false;
            cardButtonList[firstGuessIndex].GetComponent<Card>().ShowFront();
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = index;
            secondGuessName = cardSOList[secondGuessIndex].cardName;

            cardButtonList[secondGuessIndex].interactable = false;
            cardButtonList[secondGuessIndex].GetComponent<Card>().ShowFront();

            countGuesses++;
            CheckCardNames();
        }
    }


    void CheckCardNames()
    {
        bool guessMatch = firstGuessName == secondGuessName;
       
        StartCoroutine(CheckCardMatch(guessMatch, cardButtonList[firstGuessIndex], cardButtonList[secondGuessIndex]));
        firstGuess = secondGuess = false;
        
    }
    IEnumerator CheckCardMatch(bool IsMatched, Button firstCard, Button secondCard)
    {
        if (IsMatched)
        {
            //firstCard.interactable = false;
            //secondCard.interactable = false;

            yield return new WaitForSeconds(1f);

            firstCard.GetComponent<Card>().HideCard();
            secondCard.GetComponent<Card>().HideCard();

            countCorrectGuesses++;
            CheckIfGameIsFinished();
        }
        else
        {
            yield return new WaitForSeconds(1f);

            firstCard.GetComponent<Card>().ShowBack();
            secondCard.GetComponent<Card>().ShowBack();

            firstCard.interactable = true;
            secondCard.interactable = true;

            //firstCard.image.sprite = bgSprite;
            //secondCard.image.sprite = bgSprite;
        }
    }

    private void CheckIfGameIsFinished()
    {
        if(countCorrectGuesses >= gameGuesses)
        {
            Debug.Log("Game Complete");
        }
    }

    private void Shuffle(List<CardScriptableObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            CardScriptableObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
            cardButtonList[i].GetComponent<Card>().cardSO = list[i];
        }
    }
}
