using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Card Data")]
    public CardScriptableObject cardData;

    [Header("UI References")]
    private Image cardFrontImage;

    //public Text cardNameText; // Optional if you want name display

    private void Start()
    {
        if (cardData != null)
        {
            ApplyCardData();
        }
        else
        {
            Debug.LogWarning($"CardData not set on {gameObject.name}");
        }
    }

    public void ApplyCardData()
    {
        if (cardFrontImage != null)
            cardFrontImage.sprite = cardData.cardSprite;
    }


    public string GetCardName()
    {
        return cardData != null ? cardData.cardName : "Unknown";
    }
}
