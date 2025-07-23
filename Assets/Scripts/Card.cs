using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;
    public bool IsMatched { get; set; }

    [SerializeField] private Image cardFrontImage;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (cardSO != null && cardFrontImage != null)
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
        animator.SetBool("Back", false);
        animator.SetBool("Front", true);
    }

    public void ShowBack()
    {
        animator.SetBool("Front", false);
        animator.SetBool("Back", true);
    }

    public void HideCard()
    {
        GetComponent<Image>().enabled = false;
        cardFrontImage.enabled = false;
    }

    public void FadeCard()
    {
        StartCoroutine(FadeOutCardWithCanvasGroup());
    }

    private IEnumerator FadeOutCardWithCanvasGroup()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = gameObject.AddComponent<CanvasGroup>();
        }

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }

        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

}
