using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Objects/Card")]
public class CardScriptableObject : ScriptableObject
{
    public string cardName;

    public Sprite cardSprite;

}
