using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    [SerializeField] private Image cardFrontImage;

    private void Start()
    {
        if (cardSO != null)
        {
            cardFrontImage.sprite = cardSO.cardSprite;
        }
    }

    public void SetCardSprite(Sprite sprite)
    {
        cardFrontImage.sprite = sprite;
    }

    public void ShowFront()
    {
        GetComponent<Animator>().SetBool("Back", false);
        GetComponent<Animator>().SetBool("Front", true);
    }

    public void ShowBack()
    {
        GetComponent<Animator>().SetBool("Front", false);
        GetComponent<Animator>().SetBool("Back", true);
    }

    public void HideCard()
    {
        GetComponent<Image>().enabled = false;
    }
}
