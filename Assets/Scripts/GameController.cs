using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        StartCoroutine(PreviewCardsCoroutine());
    }

    private void ApplyCardSprites(List<Button> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            Card cardComponent = cardList[i].GetComponent<Card>();
            if (cardComponent != null)
            {
                cardComponent.cardSO = cardSOList[i];
                cardComponent.SetCardSprite(cardSOList[i].cardSprite);
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
    IEnumerator CheckCardMatch(bool isMatched, Button firstCard, Button secondCard)
    {
        if (isMatched)
        {
            //firstCard.interactable = false;
            //secondCard.interactable = false;

            yield return new WaitForSeconds(1f);

            firstCard.GetComponent<Card>().FadeCard();
            secondCard.GetComponent<Card>().FadeCard();

            firstCard.GetComponent<Card>().IsMatched = true;
            secondCard.GetComponent<Card>().IsMatched = true;

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

    IEnumerator PreviewCardsCoroutine()
    {
        int previeTimer = 2;
        // Step 1: Flip all cards to front
        foreach (var button in cardButtonList)
        {
            button.GetComponent<Card>().ShowFront();
            button.interactable = false; 
        }

        if(GameSettings.Instance && GameSettings.Instance.SelectedGridSize == GridEnum.GridSize.Grid_5x6)
            previeTimer = 4;
        yield return new WaitForSeconds(previeTimer);

        
        foreach (var button in cardButtonList)
        {
            button.GetComponent<Card>().ShowBack();
            button.interactable = true; // now allow player to click
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
            var temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void SaveGame()
    {
        List<Card> cards = new List<Card>();
        foreach (Button btn in cardButtonList)
        {
            Card card = btn.GetComponent<Card>();
            if (card != null)
                cards.Add(card);
        }

        GameSettings.Instance.SaveGame(cards);
    }

    public void LoadGame()
    {
        var loadedCards = GameSettings.Instance.LoadGame();
        if (loadedCards == null || loadedCards.Count == 0)
            return;

        cardSOList.Clear();

        foreach (var savedCard in loadedCards)
        {
            CardScriptableObject cardSO = System.Array.Find(cardSOArray, c => c.cardName == savedCard.cardName);
            if (cardSO != null)
            {
                cardSOList.Add(cardSO);
            }
            else
            {
                Debug.LogWarning("Card not found in resources: " + savedCard.cardName);
            }
        }

        ApplyCardSprites(cardButtonList);

        for (int i = 0; i < loadedCards.Count; i++)
        {
            Card card = cardButtonList[i].GetComponent<Card>();
            if (card != null)
            {
                card.IsMatched = loadedCards[i].isMatched;

                if (card.IsMatched)
                {
                    card.HideCard();
                    cardButtonList[i].interactable = false;
                }
                else
                {
                    card.ShowBack();
                    cardButtonList[i].interactable = true;
                }
            }
        }

        Debug.Log("Game state applied from saved data.");
    }

}
