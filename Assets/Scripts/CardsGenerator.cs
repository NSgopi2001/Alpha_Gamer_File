using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsGenerator : MonoBehaviour
{
    [SerializeField] private Transform gridPanel;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private int numberOfCards = 6;

    private void Awake()
    {
        GenerateCards();
    }

    private void GenerateCards()
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject card = Instantiate(cardPrefab, gridPanel);
            card.name = "" + i;
        }
    }
}
