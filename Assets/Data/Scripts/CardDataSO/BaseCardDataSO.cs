using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Card/Base Test Card")]

public abstract class BaseCardDataSO : ScriptableObject
{
    public string title;
    public Sprite cardSprite;
    public string type;
    public string description;
    public int cost;
    public GameObject cardPrefab;
    public Resources resourceRequired;
}
