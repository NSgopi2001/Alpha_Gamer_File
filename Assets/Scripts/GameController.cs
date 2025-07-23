using System;
using System.Collections;
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

        gameGuesses = cardSOList.Count / 2;
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
                cardButtonList.Add(btn);
            }
            else
            {
                Debug.LogWarning($"GameObject {cards.name} is tagged 'Card' but has no Button component.");
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
            //cardsList[i].GetComponent<Card>().cardData = cardSOList[index];
            index++;
        }

    }

    private void AddListeners()
    {
        foreach (Button cardButtons in cardButtonList)
        {
            cardButtons.onClick.AddListener(() => PickCard());
        }
    }

    private void PickCard()
    {
        string _cardName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        if (!firstGuess)
        {
            firstGuess = true;

            firstGuessIndex = int.Parse(_cardName);

            firstGuessName = cardSOList[firstGuessIndex].name;

            cardButtonList[firstGuessIndex].image.sprite = cardSOList[firstGuessIndex].cardSprite;
            cardButtonList[firstGuessIndex].interactable = false;
            cardButtonList[firstGuessIndex].animator.SetBool("Front", true);
        }
        else if (!secondGuess)
        {
            secondGuess = true;

            secondGuessIndex = int.Parse(_cardName);

            secondGuessName = cardSOList[secondGuessIndex].name;

            cardButtonList[secondGuessIndex].image.sprite = cardSOList[secondGuessIndex].cardSprite;
            cardButtonList[secondGuessIndex].interactable = false;
            cardButtonList[secondGuessIndex].animator.SetBool("Front", true);

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

            yield return new WaitForSeconds(0.5f);

            firstCard.image.enabled = false;
            secondCard.image.enabled = false;

            countCorrectGuesses++;
            CheckIfGameIsFinished();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

            firstCard.interactable = true;
            secondCard.interactable = true;

            firstCard.image.sprite = bgSprite;
            secondCard.image.sprite = bgSprite;
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
        }
    }
}
